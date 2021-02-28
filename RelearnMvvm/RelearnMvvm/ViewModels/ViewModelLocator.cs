using GalaSoft.MvvmLight.Ioc;
using RelearnMvvm.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RelearnMvvm.ViewModels
{
    public class ViewModelLocator
    {
        public PoetriesViewModel PoetriesViewModel =>
            SimpleIoc.Default.GetInstance<PoetriesViewModel>();

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<IPoetryStorage, PoetryStorage>();
            SimpleIoc.Default.Register<PoetriesViewModel>();
        }
    }
}
