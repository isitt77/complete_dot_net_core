using System;
namespace CompleteDotNetCore.Utility
{
    public static class SD
    {
        // Roles
        public const string RoleUserIndv = "Individual";
        public const string RoleUserComp = "Company";
        public const string RoleAdmin = "Admin";
        public const string RoleEmployee = "Employee";

        // Order Status
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        // Payment Status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";

        // Session Variable
        public const string SessionCart = "SessionShoppingCart";
    }
}

