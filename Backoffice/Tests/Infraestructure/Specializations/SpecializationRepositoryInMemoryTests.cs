using System;
using Backoffice.Domain.Specializations;
using Xunit;
using Backoffice.Infraestructure.Specializations;
using Backoffice.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Tests;

public class SpecializationRepositoryInMemoryTests
{
    private readonly SpecializationRepository _specializationRepository;
    private readonly BDContext _context;

    public SpecializationRepositoryInMemoryTests()
    {
        var options = new DbContextOptionsBuilder<BDContext>()
                .UseInMemoryDatabase(databaseName: "HealthcareAppTestDb")
                .Options;
        _context = new BDContext(options);
        _specializationRepository = new SpecializationRepository(_context);

    }

    [Fact]
    public async Task TestSpecializationNameExistsWithDBInMemory()
    {
        _context.Specializations.RemoveRange(_context.Specializations);
        await _context.SaveChangesAsync();

        var specialization = new Specialization(new SpecializationName("Cardiology"));
        _context.Specializations.Add(specialization);
        await _context.SaveChangesAsync();

        var exists = await _specializationRepository.SpecializationNameExists("Cardiology");

        Assert.True(exists);
    }

    [Fact]
    public async Task TestSpecializationNameDoesNotExistWithDBInMemory()
    {
        _context.Specializations.RemoveRange(_context.Specializations);
        await _context.SaveChangesAsync();

        var specialization = new Specialization(new SpecializationName("Cardiology"));
        _context.Specializations.Add(specialization);
        await _context.SaveChangesAsync();

        var exists = await _specializationRepository.SpecializationNameExists("NonExistentSpecialization");

        Assert.False(exists);
    }

    [Fact]
    public async Task TestGetSpecializationByNameWithDBInMemory()
    {
        _context.Specializations.RemoveRange(_context.Specializations);
        await _context.SaveChangesAsync();

        var specialization = new Specialization(new SpecializationName("Cardiology"));
        _context.Specializations.Add(specialization);
        await _context.SaveChangesAsync();

        var result = await _specializationRepository.GetBySpecializationName("Cardiology");

        Assert.NotNull(result);
        Assert.Equal("Cardiology", result.Name.Name);
    }

    [Fact]
    public async Task TestGetSpecializationByNameNotFoundWithDBInMemory()
    {
        _context.Specializations.RemoveRange(_context.Specializations);
        await _context.SaveChangesAsync();

        var specialization = new Specialization(new SpecializationName("Cardiology"));
        _context.Specializations.Add(specialization);
        await _context.SaveChangesAsync();

        var result = await _specializationRepository.GetBySpecializationName("NonExistentSpecialization");

        Assert.Null(result);
    }
}
