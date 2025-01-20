# ai-cachara

## About the Project:

This FullStack application serves as a training for AI concepts and structure within the .NET environment.
It also serves as a training ground for frontend development using typescript, react and Next.js.

In the end of this project, the application should:

1. Classify document píctures quality(good, medium, poor);
2. Extract data from the documents with:
    1. LLMs APIs (semantic Kernel);
    2. Custom AI Local Model, with training data (ML.NET).
3. Store the pictures and extrated data into a non-relational database;
4. Have a Next.js Interface in which you can upload files for classification;

### Technologies

1. .NET 9;
2. ML.NET and Semantic Kernel;
3. Non-relational database;
3. React in Next.js;
4. Typescript;
5. Consistent coding style with .editorconfig file 

# Project Structure

## 1. Backend

### AICachara.Core

Application Services, Entities, Infrastructure and ML related code

### AICachara.API

Endpoints for file upload, testing and providing content for the UI

## 2. Frontend

### ai-cachara-ui

Next.js application that provides the User Interface of this application.


