using AutoMapper;
using ChatApplication.DAL.Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace ChatApplication.BLL.Tests.Services.ChatService;

public class ChatServiceBaseSetup
{
    protected Mock<IUnitOfWork> _mockUnitOfWork;
    protected Mock<IMapper> _mockMapper;
    protected BLL.Services.ChatService _sut;
    
    [OneTimeSetUp]
    public void BaseInit()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _sut = new BLL.Services.ChatService(
            _mockUnitOfWork.Object,
            _mockMapper.Object);
    }
}