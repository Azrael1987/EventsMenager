using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Handlers;
using Evento.Infrastructure.Services;
using Evento.Infrastructure.Settings;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Evento.Tests.Services
{
    public class UserServiceTests
    {
        private void Init()
        {
            //_jwtHandlerMock = new Mock<IJwtHandler>();
            //_mapperMock = new Mock<IMapper>();
            //_userRepositoryMock = new Mock<IUserRepository>();
            //_eventRepositoryMock = new Mock<IEventRepository>();
            //_userService = new UserService(_userRepositoryMock.Object, _jwtHandlerMock.Object, _mapperMock.Object);
            //_ticketService = new TicketService(_userRepositoryMock.Object, _eventRepositoryMock.Object, _mapperMock.Object);
            //_accountController = new AccountController(_userService, _ticketService);
            //_user = new User(Guid.NewGuid(), "user", "NewUser", "NewUser2@poczta.pl", "tajneHasło254%$");
            //_accountDto = new AccountDto
            //{
            //    Id = _user.Id,
            //    Email = _user.Email,
            //    Name = _user.Name,
            //    Role = _user.Role,
            //    Password = _user.Password,
            //    CreatesAt = _user.CreatesAt
            //};
            //_tokenDto = new TokenDto
            //{
            //    Token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI2MzI3MjA3OS1kMWY2LTQ5MTQtYmNiZC0zNDQ0NmQ2NDIyZTkiLCJ1bmlxdWVfbmFtZSI6IjYzMjcyMDc5LWQxZjYtNDkxNC1iY2JkLTM0NDQ2ZDY0MjJlOSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImFkbWluIiwianRpIjoiNjMyNzIwNzktZDFmNi00OTE0LWJjYmQtMzQ0NDZkNjQyMmU5IiwiaWF0IjoiMTU3MTY4ODMxNjQwNSIsIm5iZiI6MTU3MTY4ODMxNiwiZXhwIjoxNTcxNjk1NTE2LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjQ0MzA4In0.1KrvTNNl5nlHKb6jcKR5iLJTp2rNT7PmTuptuVzBEAQ",
            //    Expires = 120,
            //    Role = "user",
            //};
        }

        [Fact]
        public async Task RegisterAsyncShouldInvokeAddAsyncOnUserRepository()
        {
            // Arrange
            var _jwtHandlerMock = new Mock<IJwtHandler>();
            var _mapperMock = new Mock<IMapper>();
            var _userRepositoryMock = new Mock<IUserRepository>();
            var _userService = new UserService(_userRepositoryMock.Object, _jwtHandlerMock.Object, _mapperMock.Object);

            // Act
            await _userService.RegistryAsync(Guid.NewGuid(), "test1@poczta.pl", "test", "123qweASD!@#", "user");

            // Action
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task WhenInvokingGetAsyncWithIdThanItInvokeGetAsyncUserRepository()
        {
            // Arrange
            var _userRepositoryMock = new Mock<IUserRepository>(); // tego chyba nie mozna du inita
            var _mapperMock = new Mock<IMapper>(); // tego chyba nie mozna do inita
            var _jwtHandlerMock = new Mock<IJwtHandler>();
            var _userService = new UserService(_userRepositoryMock.Object, _jwtHandlerMock.Object, _mapperMock.Object);
            var _user = new User(Guid.NewGuid(), "user", "NewUser", "NewUser2@poczta.pl", "tajneHasło254%$");
            var _accountDto = new AccountDto
            {
                Id = _user.Id,
                Email = _user.Email,
                Name = _user.Name,
                Role = _user.Role,
                Password = _user.Password,
                CreatesAt = _user.CreatesAt
            };
            _userRepositoryMock.Setup(x => x.GetAsync(_user.Id)).ReturnsAsync(_user);
            _mapperMock.Setup(x => x.Map<AccountDto>(_user)).Returns(_accountDto);

            // Act
            var existingAcountDto = await _userService.GetAccountAsync(_user.Id);

            // Action
            _userRepositoryMock.Verify(x => x.GetAsync(_user.Id), Times.Once());
            existingAcountDto.Should().NotBeNull();
            existingAcountDto.Id.Equals(_user.Id);
            existingAcountDto.Name.Equals(_user.Name);
            existingAcountDto.Email.Equals(_user.Email);
            existingAcountDto.Password.Should().BeNull(); // We protecting password and never show them ;)
            existingAcountDto.Role.Equals(_user.Role);
            existingAcountDto.CreatesAt.Equals(_user.CreatesAt);
            existingAcountDto.Should().Equals(_accountDto);
        }

        [Fact]
        public async Task WhenInvokingGetForEmailAsyncThanItInvokeGetAsyncUserRepository()
        {
            // Arrage
            var _userRepositoryMock = new Mock<IUserRepository>();
            var _mapperMock = new Mock<IMapper>();
            var _jwtHandlerMock = new Mock<IJwtHandler>();
            var _userService = new UserService(_userRepositoryMock.Object, _jwtHandlerMock.Object, _mapperMock.Object);
            var _user = new User(Guid.NewGuid(), "user", "NewUser", "NewUser2@poczta.pl", "tajneHasło254%$");
            var _accountDto = new AccountDto
            {
                Id = _user.Id,
                Email = _user.Email,
                Name = _user.Name,
                Role = _user.Role,
                Password = _user.Password,
                CreatesAt = _user.CreatesAt
            };
            _userRepositoryMock.Setup(x => x.GetForEmailAsync(_user.Email)).ReturnsAsync(_user);
            _mapperMock.Setup(x => x.Map<AccountDto>(_user)).Returns(_accountDto);

            // Act
            var existingAcountDto = await _userService.GetAccountAsync(_user.Email);

            // Action
            _userRepositoryMock.Verify(x => x.GetForEmailAsync(_user.Email), Times.Once());
            existingAcountDto.Should().NotBeNull();
            existingAcountDto.Id.Equals(_user.Id);
            existingAcountDto.Name.Equals(_user.Name);
            existingAcountDto.Email.Equals(_user.Email);
            existingAcountDto.Password.Should().BeNull(); // We protecting password and never show them ;)
            existingAcountDto.Role.Equals(_user.Role);
            existingAcountDto.CreatesAt.Equals(_user.CreatesAt);
            existingAcountDto.Should().Equals(_accountDto);
        }

        [Fact]
        public async Task AfterLogInReturnsToken()
        {
            // Arrage
            Guid guid = Guid.NewGuid();
            var _userRepositoryMock = new Mock<IUserRepository>();
            var _mapperMock = new Mock<IMapper>();
            var _ioptions = new OptionsMock<JwtSettings>();
            _ioptions.Value = new JwtSettings() { ExpiryMinutes = 120, Issuer = "http://localhost:44308", Key = "123eret-432rew-43ddd-34342" };
            var _jwtHandler = new JwtHandler(_ioptions);
            var _user = new User(guid, "user", "test", "test1@poczta.pl", "123qweASD!@#"); // zmienic kolejnosc parametów w konstruktorze  objektu User
            var _accountDto = new AccountDto
            {
                Id = _user.Id,
                Email = _user.Email,
                Name = _user.Name,
                Role = _user.Role,
                Password = _user.Password,
                CreatesAt = _user.CreatesAt
            };
            var _jwtDto = new TokenDto
            {
                Token = "12345-qwer-4567-@@@##$",
                Expires = 120000,
                Role = "user"
            };
            var _userService = new UserService(_userRepositoryMock.Object, _jwtHandler, _mapperMock.Object);

            _userRepositoryMock.Setup(x => x.GetForEmailAsync("test1@poczta.pl")).ReturnsAsync(_user);
            _mapperMock.Setup(x => x.Map<AccountDto>(_user)).Returns(_accountDto);
            //  _jwtHandlerMock.Setup(x => x.CreateToken(_user.Id, _user.Role)).Returns(_jwtDto);

            // Act
            //  await _userService.RegistryAsync(guid, "test1@poczta.pl", "test", "123qweASD!@#", "user");
            var existingAcountDto = await _userService.GetAccountAsync("test1@poczta.pl");
            var token = await _userService.LoginAsync("test1@poczta.pl", "123qweASD!@#");

            // Assert
            token.Should().NotBeNull();
            Assert.Same(_user.Role, token.Role);
            token.Expires.Should().BeGreaterThan(0);
            token.Token.Length.Should().Equals(543);
            token.Token.Should().Contain(".");

            _userRepositoryMock.Verify(x => x.GetForEmailAsync("test1@poczta.pl"), Times.Exactly(2));
        }

        private class OptionsMock<T> : IOptions<T> where T : class, new()
        {
            public T Value { get; set; }
        }
        //WhenInvokingLoginAsyncThanGenerateToken()
    }
}
