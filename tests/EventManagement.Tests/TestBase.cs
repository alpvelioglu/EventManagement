using AutoMapper;
using EventManagement.Core.Interfaces;
using Moq;
using NUnit.Framework;

namespace EventManagement.Tests;

public abstract class TestBase
{
    protected Mock<IMapper> MockMapper;
    protected Mock<IErrorLogger> MockErrorLogger;
    protected Mock<IUnitOfWork> MockUnitOfWork;

    [SetUp]
    public virtual void Setup()
    {
        MockMapper = new Mock<IMapper>();
        MockErrorLogger = new Mock<IErrorLogger>();
        MockUnitOfWork = new Mock<IUnitOfWork>();
    }
}
