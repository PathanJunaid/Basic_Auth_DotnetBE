using Basic_Auth.Model;
using Basic_Auth.Model.dto;
using Basic_Auth.Model.Entities;

namespace Basic_Auth.Services
{
    public class UserServices : IUserService
    {
        private readonly AppDbContext dbcontext;

        public UserServices(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<User> CreateUserAsync(Userdto user)
        {
            var existingUser = await FindUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return null;
            }
            user.Password = HashPassword(user.Password);
            User userdata = new()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };

            await dbcontext.Users.AddAsync(userdata);
            await dbcontext.SaveChangesAsync();
            return userdata;
        }

        public async Task<User> UpdateUserAsync(Guid id, string Name)
        {
            var user = await FindUserAsync(id);
            if (user == null)
            {
                return null;
            }
            user.Name = Name;
            dbcontext.Users.Update(user);
            await dbcontext.SaveChangesAsync();
            return user;   
        }

        public async Task<string> DeleteUserAsync(Guid id)
        {
            var user = await FindUserAsync(id);
            if (user == null)
            {
                return $"User {id} not found";
            }
            dbcontext.Users.Remove(user);
            await dbcontext.SaveChangesAsync();
            return $"User {id} deleted successfully.";
        }
        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await Task.Run(() => dbcontext.Users.FirstOrDefault(u => u.Email == email));
        }

        public async Task<User?> FindUserAsync(Guid id)
        {
            return await Task.Run(() => dbcontext.Users.FirstOrDefault(u => u.Id == id));
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12); // Secure bcrypt hashing
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashedPassword);
        }
    }
}
