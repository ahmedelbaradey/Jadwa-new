
namespace Abstraction.Constants
{
    public static partial class Claims
    {

        public static List<string> GenerateModules()
        {
            return new List<string>()
            {
                "Product",
                "Strategy",
                "DemoEntity",
                "Resolution",
                "BoardMember",
               // "Fund",
            };
        }
        public static List<string> GenerateFundManagerModules()
        {
            return new List<string>()
            {
                "Fund.View",
                "Fund.List",
                "Fund.Create",
                "Fund.EditFundExitdate",
                // Resolution permissions for Fund Manager
                "Resolution.View",
                "Resolution.List",
                "Resolution.Create",
                "Resolution.Edit",
                "Resolution.Delete",
                "Resolution.Cancel",
                // Board Member permissions for Fund Manager
                "BoardMember.Edit",
                "BoardMember.Delete",
            };
        }
        public static List<string> GenerateBoardSecertaryModules()
        {
            return new List<string>()
            {
                "Fund.View",
                "Fund.List",
                //"Fund.Create",
                //"Fund.Edit",
                "Fund.EditFundExitdate",
                // Resolution permissions for Board Secretary (View only)
                "Resolution.View",
                "Resolution.List",
                 "Resolution.Edit",
                // Board Member permissions for Fund Manager
                "BoardMember.Create",
            };
        }
        public static List<string> GenerateLegalCouncilModules()
        {
            return new List<string>()
            {

                "Fund.Edit",
                "Fund.View",
                "Fund.List",
                "Fund.Create",
                "Fund.Complete",
                "Fund.EditFundExitdate",
                // Resolution permissions for Legal Council (View only)
                "Resolution.View",
                "Resolution.List",
                "Resolution.Edit",
                // Board Member permissions for Fund Manager
                "BoardMember.Create",

    };
        }
        public static List<string> GenerateAdminModules()
        {
            return new List<string>()
            {

                "Strategy.Edit",
                "Strategy.View",
                "Strategy.List",
                "Strategy.Create",

    };
        }
        public static List<string> GeneratePermissions(string module)
        {
            var permissions = new List<string>()
            {
                $"{module}.View",
                $"{module}.List",
                $"{module}.Create",
                $"{module}.Edit",
                $"{module}.Delete"
            };

            // Add Cancel permission for Resolution module
            if (module == "Resolution")
            {
                permissions.Add($"{module}.Cancel");
            }
            // Add Edit Fund Exitdate permission for Fund module
            if (module == "Fund")
            {
                permissions.Add($"{module}.EditFundExitdate");
            }


            return permissions;
        }

    }
}