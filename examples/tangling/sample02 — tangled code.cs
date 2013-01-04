[OperationBehavior(TransactionScopeRequired = true)]
public Status LogIn(string emailAddress, string password, string campaignId, string affiliateId, string accessKeyId, string signatureHash)
{
    Guid? authUserId;

    if (!CheckAuthorization(accessKeyId, CalculateSignature(emailAddress, password, campaignId, affiliateId, accessKeyId, signatureHash), signatureHash, out authUserId))
    {
        return new Status { Success = false, Description = AuthorizationStatus.AuthorizationFailed };
    }

    var result = new Status { Success = false };

    var currentUserId = Guid.Empty;

    emailAddress = emailAddress.SafeTrim();
    password = password.SafeTrim();

    if (!emailAddress.IsEmail())
    {
        result.Description = VerificationStatus.NotRegisteredEmail;

        setResponseStatusCode(HttpStatusCode.Forbidden);
    }
    else if (!password.IsPassword())
    {
        result.Description = VerificationStatus.IncorrectPassword;

        setResponseStatusCode(HttpStatusCode.Forbidden);
    }
    else
    {
        var currentPasswordHash = EncodePasswordToBase64(password);

        var currentUser = context.Users.SingleOrDefault(user => (user.EmailAddress == emailAddress));

        if (currentUser != null)
        {
            if (currentUser.PasswordHash == currentPasswordHash)
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionTimeout))
                {
                    var accessKey = GetAccessKey(Guid.Parse(accessKeyId));

                    SetUserId2AccessKey(accessKey.Id, currentUser.Id);

                    LinkApplicationUsers(currentUser, accessKey);

                    saveChangesStrategy.SaveChanges(context);

                    scope.Complete();
                }

                currentUserId = currentUser.Id;

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
    }

    if (result.Success)
    {
        processAffiliateIfNecessary(currentUserId, campaignId, affiliateId);
    }

    return result;
}