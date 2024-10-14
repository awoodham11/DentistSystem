namespace BlazorApp1.Identity.Services
{
	public class Register
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }

		public DateOnly dob { get; set; }

		public string addData()
		{
			//post to API
			return "Data for new user here";
		}
	}
}
