using AutoMapper;
using ChatApplication.BLL.Services;
using ChatApplication.DAL.Data.Interfaces;
using ChatApplication.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.UserService;

public class UserServiceBaseSetup
{
    protected Mock<IUnitOfWork> _mockUnitOfWork;
    protected Mock<IMapper> _mockMapper;
    protected Mock<AzureBlobStorageService> _mockBlobStorageService;
    protected BLL.Services.UserService _sut;
    
    [OneTimeSetUp]
    public void BaseInit()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockBlobStorageService = new Mock<AzureBlobStorageService>();

        _sut = new BLL.Services.UserService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockBlobStorageService.Object);
    }
}