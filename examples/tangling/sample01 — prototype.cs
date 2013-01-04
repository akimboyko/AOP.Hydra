public UserIdDTO LogOnUserMobile (string applicationId, string accessToken, string userId)
{
    try
    {
        var appId = Guid.Parse(applicationId);
        Application application = _context.Applications.SingleOrDefault(x => x.Id == appId);
        if (application == null)
        {
            return new UserIdDTO() {Error = "Wrong applicationId"};
        }

        var socialMediaUser = new Model.ApplicationUser
        {
            AccessToken = accessToken,
        };

        var engine = EngineFactory.GetEngine(application);
        socialMediaUser = engine.LogOnUser(application, socialMediaUser);
        return new UserIdDTO() { UserId = socialMediaUser.UserId.ToString() };

    }
    catch (Exception e)
    {
        return new UserIdDTO() { Error = "Shit.." };
    }
}