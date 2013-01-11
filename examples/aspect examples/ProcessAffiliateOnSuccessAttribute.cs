using System;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using MyCompany.Database;
using MyCompany.Engine.Core;
using MyCompany.Services.Contracts;

namespace MyCompany.Services.Aspect
{
    [Serializable]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Tracing)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Security)]
    public class ProcessAffiliateOnSuccessAttribute : OnMethodBoundaryAspect
    {
        public override bool CompileTimeValidate(MethodBase method)
        {
            var result = true;
            var methodInfo = method as MethodInfo;

            if (!typeof(AbstractService).IsAssignableFrom(method.DeclaringType))
            {
                result &= false;

                Message.Write(methodInfo, SeverityType.Error, "998", "Only class derived from Abstract Service allowed");
            }

            if (!typeof(Status).IsAssignableFrom(methodInfo.ReturnType))
            {
                result &= false;

                Message.Write(methodInfo, SeverityType.Error, "997", "Methods with return type {0} cannot be used", methodInfo.ReturnType.Name);
            }

            return result;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var status = args.ReturnValue as Status;

            if (status != null && status.Success)
            {
                var campaignId = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "campaignId"));
                var affiliateId = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "affiliateId"));
                var accessKeyId = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "accessKeyId"));

                Guid validCampaignId;
                Guid validAccessKeyId;

                if (Guid.TryParse(campaignId, out validCampaignId) && Guid.TryParse(accessKeyId, out validAccessKeyId))
                {
                    using (var context = new DataContext())
                    {
                        var accessKey = context.AccessKeys.SingleOrDefault(key => key.Id == validAccessKeyId);

                        if (accessKey != null && accessKey.UserId != null)
                        {
                            processAffiliate(context, accessKey.UserId ?? Guid.Empty, validCampaignId, affiliateId);
                        }
                    }
                }
            }
        }

        static void processAffiliate(DataContext context, Guid currentUserId, Guid campaignId, string affiliateId)
        {
            if (campaignId != Guid.Empty && currentUserId != Guid.Empty)
            {
                EngineFactory.GetAffiliateEngine(context).AddAffiliateIfNecessary(currentUserId, campaignId, affiliateId);
            }
        }
    }
}
