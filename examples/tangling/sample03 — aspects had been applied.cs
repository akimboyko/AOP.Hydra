[SignatureVerificationStatus(Success = false, Description = AuthorizationStatus.AuthorizationFailed, AspectPriority = 0)]
[ParametersVerification(SafeTrimming = true, AspectPriority = 1)]
[ProcessAffiliateOnSuccess(AspectPriority = 2)]
[TransactionScope(AspectPriority = 3)] // http://www.sharpcrafters.com/solutions/transaction
[OperationBehavior(TransactionScopeRequired = true)]
public Status LogIn(string emailAddress, string password, string campaignId, string affiliateId, string nonce, string accessKeyId, string signatureHash)
{
    var result = new Status { Success = false, Nonce = nonce };

    var currentUser = context.Users.SingleOrDefault(user => (user.EmailAddress == emailAddress));

    if (currentUser != null)
    {
        if (ComparePasswordHashes(currentUser.PasswordHash, password))
        {
            var accessKey = GetAccessKey(Guid.Parse(accessKeyId));

            SetUserId2AccessKey(accessKey.Id, currentUser.Id);

            LinkApplicationUsers(currentUser, accessKey);

            result.Success = true;
            result.Description = VerificationStatus.Ok;
        }
        else
        {
            result.Description = VerificationStatus.IncorrectPassword;

            setResponseStatusCode(HttpStatusCode.Forbidden);
        }
    }
    else
    {
        result.Description = VerificationStatus.InvalidEmail;

        setResponseStatusCode(HttpStatusCode.Forbidden);
    }

    return result;
}
