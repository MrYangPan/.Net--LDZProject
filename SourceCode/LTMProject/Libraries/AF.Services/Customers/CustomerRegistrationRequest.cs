using AF.Domain.Domain.Customers;

namespace AF.Services.Customers
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public class CustomerRegistrationRequest
    {
        /// <summary>
        /// Customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Password format
        /// </summary>
        public PasswordFormat PasswordFormat { get; set; }


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="passwordFormat">Password fprmat</param>
        public CustomerRegistrationRequest(Customer customer,  string username,
            string password, 
            PasswordFormat passwordFormat)
        {
            this.Customer = customer;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
        }
    }
}
