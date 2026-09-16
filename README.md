# Agenda de Contactos - .NET MAUI

Este repositorio contiene una aplicación móvil para la gestión de contactos, desarrollada como actividad práctica. El proyecto demuestra la implementación de operaciones CRUD utilizando una base de datos local (**SQLite**) bajo una arquitectura estrictamente orientada al patrón **MVVM** (Model-View-ViewModel).

## 🚀 Características Principales

* **Operaciones CRUD Completas:** Permite listar, buscar, agregar, editar y eliminar contactos.
* **Búsqueda Dinámica:** Filtrado en tiempo real de contactos por nombre mediante `SearchBar`.
* **Persistencia Local:** Uso de `sqlite-net-pcl` para almacenar datos de forma persistente en el dispositivo.
* **Siembra de Datos (Seed):** Inicialización automática de la base de datos con contactos de prueba en la primera ejecución.
* **Navegación Modal:** Flujo de usuario ininterrumpido utilizando modales de MAUI Shell para la creación y edición de registros, evitando recargas innecesarias.
* **Validación de Datos:** Reglas de negocio aisladas para validar formatos de correo electrónico, longitud de teléfonos y campos obligatorios, con feedback visual al usuario.

## 🏗️ Arquitectura y Patrones de Diseño

El proyecto está diseñado pensando en la escalabilidad y el código limpio:

* **MVVM (Model-View-ViewModel):** Separación total entre la lógica de negocio y la interfaz de usuario, potenciado por `CommunityToolkit.Mvvm` para reducir el código repetitivo (*boilerplate*).
* **Patrón Repositorio:** Toda la interacción con SQLite está centralizada en `ContactoRepository`. La base de datos es la única fuente de verdad; no se utilizan Singletons para mantener el estado en memoria.
* **Inyección de Dependencias:** Registro centralizado en `MauiProgram.cs` para el repositorio (Singleton) y las vistas/ViewModels (Transient).
* **Paso de Parámetros Seguro:** Uso de `IQueryAttributable` para transferir datos complejos de forma segura durante la navegación de Shell.

## 🛠️ Tecnologías y Librerías

* **Framework:** .NET MAUI (.NET 9.0)
* **Lenguaje:** C# 13 / XAML
* **Base de Datos:** SQLite (`sqlite-net-pcl`, `SQLitePCLRaw.bundle_green`)
* **Herramientas MVVM:** `CommunityToolkit.Mvvm`

## 📁 Estructura del Proyecto

```text
├── Models/
│   └── Contacto.cs                 # Entidad de base de datos (ORM)
├── Data/
│   └── ContactoRepository.cs       # Conexión asíncrona a SQLite y operaciones CRUD
├── Helpers/
│   ├── ValidacionHelper.cs         # Métodos puros para validaciones
│   └── StringToBoolConverter.cs    # Conversor para visibilidad de UI
├── ViewModels/
│   ├── ContactosViewModel.cs       # Lógica de la lista y búsqueda
│   └── ContactoDetalleViewModel.cs # Lógica del modal (Agregar/Editar)
├── Views/
│   ├── ContactosPage.xaml          # Pantalla principal (CollectionView)
│   └── ContactoModalPage.xaml      # Pantalla modal interactiva
├── AppShell.xaml                   # Definición de navegación
└── MauiProgram.cs                  # Contenedor de Inyección de Dependencias
```

## Cómo ejecutar

1. Clonar el repositorio: https://github.com/MNEscobar/EscobarMatias_ActividadPractica_AppMovil_ll.git
2. Abrir `EscobarMatias_ActividadPractica_AppMovil_II.sln` en Visual Studio 2022.
3. Restaurar los paquetes NuGet.
4. Seleccionar la plataforma de destino (Android, Windows, etc.) y ejecutar.
