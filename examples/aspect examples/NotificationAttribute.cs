using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using Snowball.Database;
using Snowball.Database.Strategy;
using Snowball.Engine.Core;
using Snowball.Model;
using Snowball.Notification;
using MyCompany.Notification.Text;
using MyCompany.Services.Contracts;
using MyCompany.Shared;
using SharingOption = MyCompany.Engine.Core.Model.SharingOption;

namespace MyCompany.Services.Aspect
{
    [Serializable]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Security)]
    public class NotificationAttribute : OnMethodBoundaryAspect
    {
        string tempateType { get; set; }

        public NotificationAttribute(string tempateType)
        {
            this.tempateType = tempateType;
        }

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

            if (typeof(Constant.MessageTempateType).GetField(tempateType, BindingFlags.Static | BindingFlags.Public) == null)
            {
                result &= false;

                Message.Write(methodInfo, SeverityType.Error, "996", "Template Type {0} undefined", tempateType);
            }

            return result;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var returnStatus = args.ReturnValue as Status;

            if (returnStatus == null || !returnStatus.Success) return;

	    // Note: password retreived from method's arguments, database stores only hash
            var password = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "password"));
            var recipients = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "recipients"));
            var optionalText = AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "optionalText"));

            Guid campaignId;
            Guid.TryParse(AspectUtility.GetArgument<string>(args, AspectUtility.GetArgumentPosition(args, "campaignId")), out campaignId);

            var abstractService = args.Instance as AbstractService;

            if (abstractService != null)
            {
                if(!abstractService.AuthenticatedUserId.HasValue)
                    throw new ArgumentException("AbstractService.AuthenticatedUserId should not be null, i.e. only authorized users allowed");

                if (notificationCondition(args))
                {
                    sendRegistrationMessage(abstractService.context, abstractService.AuthenticatedUserId.Value, campaignId, password: password, recipients: recipients, optionalText: optionalText);
                }
            }
        }

        protected virtual bool notificationCondition(MethodExecutionArgs args)
        {
	    /* … */
            return true;
        }

        readonly static string notifyFrom = ConfigurationManager.AppSettings["NotifyFrom"];

        readonly static NotifyMessageTemplate defaultRegistrationTempate = new NotifyMessageTemplate
        {
            BodyTemplate = @"""Please contanct http://MyCompanyeffect.no""",
            SubjectTemplate = @"MyCompany Effect Configuration Issue",
            IsHtmlBodyTemplate = false,
        };

        void sendRegistrationMessage(DataContext context, Guid userId, Guid campaignId, string password = null, string recipients = null, string optionalText = null)
        {
            /* … */
        }
    }
}
