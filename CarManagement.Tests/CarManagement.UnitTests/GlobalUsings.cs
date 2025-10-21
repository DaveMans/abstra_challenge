global using Xunit;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Linq;
global using AutoMapper;
global using FluentAssertions;
global using Moq;

global using CarManagement.Domain.Entities;
global using CarManagement.Domain.Enums;
global using CarManagement.Domain.Interfaces;
global using CarManagement.Domain.Interfaces.Repositories;
global using CarManagement.Domain.Specifications;

global using CarManagement.Application.Brands.Dtos;
global using CarManagement.Application.Brands.Mapping;
global using CarManagement.Application.Brands.Commands;
global using CarManagement.Application.Brands.Queries;
global using CarManagement.Application.Brands.Specifications;
global using CarManagement.Application.Common.Pagination;
global using CarManagement.Application.Lines.Dtos;
global using CarManagement.Application.Lines.Mapping;
global using CarManagement.Application.Lines.Queries;