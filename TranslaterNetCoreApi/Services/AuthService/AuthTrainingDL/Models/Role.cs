namespace AuthTrainingDL.Models
{
    public static class Role
    {
        public const string Admin = nameof(Admin);
        public const string User = nameof(User);

        public static IEnumerable<string> AllRoles => new[] { Admin, User };
    }
}
