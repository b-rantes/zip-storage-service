using Application.UseCases.DeleteZipFile;
using Application.UseCases.DeleteZipFile.Interfaces;
using Application.UseCases.DeleteZipFile.Models;
using Application.UseCases.DeleteZipFile.Validator;
using Application.UseCases.GetZipFile;
using Application.UseCases.GetZipFile.Interfaces;
using Application.UseCases.ListZipStructureFiles;
using Application.UseCases.ListZipStructureFiles.Interfaces;
using Application.UseCases.UploadZipFile;
using Application.UseCases.UploadZipFile.Interfaces;
using Application.UseCases.ValidateZipFile;
using Application.UseCases.ValidateZipFile.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationInstaller
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUploadZipFileUseCase, UploadZipFileUseCase>();
            services.AddTransient<IGetZipFileUseCase, GetZipFileUseCase>();
            services.AddTransient<IListZipStructureFilesUseCase, ListZipStructureFilesUseCase>();
            services.AddTransient<IValidateZipFileUseCase, ValidateZipFileUseCase>();
            services.AddTransient<IDeleteZipFileUseCase, DeleteZipFileUseCase>();

            services.AddTransient<IValidator<DeleteZipFileInput>, DeleteZipFileInputValidator>();

            return services;
        }
    }
}
