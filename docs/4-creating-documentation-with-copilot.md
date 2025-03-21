# Lesson 4: Creating Documentation with Copilot

## Overview

**Goal:**  
In this lab, you will learn how to use GitHub Copilot to create and refine documentation for the **OrderingService**. You will begin by generating code-level comments using Copilot Chat and refining them with Copilot Edits. Then, you will generate project-level documentation using the **Ordering.API** project as a reference. Finally, you will learn to write API documentation for the `api/orders` resource by creating and integrating OpenAPI specifications in YAML and JSON formats.

By the end of this lab, you will have gained the following skills:

- Generating and refining code-level comments using Copilot Chat and Edits.  
- Creating project-level documentation to describe functionality and architecture.  
- Writing API documentation with OpenAPI specifications for clear and structured communication.  

**Estimated Duration:**  
30-45 minutes

**Audience:**  
 Developers, QA testers, DevOps engineers, and Technical Writers.

**Prerequisites:**  
To successfully complete this lab, ensure you have the following:  

- **Access to GitHub Copilot:** You must have an active GitHub Copilot subscription.
- **Visual Studio Code (VS Code):** Installed and set up for development.
- **GitHub Copilot Extension for VS Code:** Installed and properly configured in VS Code.
- **GitHub CLI (gh):** Installed on your local machine.
- **GitHub Copilot CLI Extension:** Installed and connected to your GitHub account via GitHub CLI.

> **Note:** If you have not completed any of the above steps, please refer to - [Lesson 1: Installing and Configuring GitHub Copilot](docs/1-installing-copilot.md) for detailed instructions.

## Code Documentation

In this step, we will use Copilot Chat add comments for all public members in the `Order.cs` class.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.Domain/AggregatesModel/OrderAggregate` folder. Open the `Order` class to add `/* */` comments
2. Open the Copilot Chat pane in Visual Studio Code by clicking the Copilot icon in the sidebar or using the command palette (`Ctrl+Alt+I`). In the chat settings, ensure the model is set to `o1-mini (Preview)`. If it’s not already selected, switch the model to `o1-mini (Preview)` in the Copilot settings.  
3. Observe that Order.cs file is already in the Working Set. (1 file). You can manage the Working Set by adding and removing files.
4. In the prompt field, enter the following prompt:  

   ```plaintext
   add /* */ comments to all public members
   ```

5. GitHub Copilot generates inline comments for every public member.
6. You might notice that the generated comments are incomplete, with a `// ...existing code...` comment at the end of the class.
7. In order to resolve this issue, click on the edit Icon at the end of the generated response in the Copilot Chat. In the Command dropdown select the last one request.
8. Copilot Edits the `Order.cs` and adds comments on all the public members in that class.
9. Review comments generated by Copilot.
10. Now lets say we wanted XML Comments, Click **Discard** button and enter the following prompt in copilot edits:

   ```plaintext
   add xml comments on all public members
   ```

*Additional Notes* - When it comes to adding comments always leverage the power of Copilot Edits that helps set a large context across multiple files.

## Project Documentation

In this step, we will use Copilot Edits to generate project documentation.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.API` folder.
2. Open the Copilot Chat pane in Visual Studio Code by clicking the Copilot icon in the sidebar or using the command palette (`Ctrl+Alt+I`). In the chat settings, ensure the model is set to `o1-mini (Preview)`. If it’s not already selected, switch the model to `o1-mini (Preview)` in the Copilot settings.  
3. Generate a readme markdown file for the project by entering the following prompt:

   ```plaintext
   Generate technical documentation for the`Ordering.API` project that can be used by business analysts to understand the scope of the project. Save it as Ordering.API.md file
   ```

4. GitHub Copilot confirms the plan and creates requested documentation to the Ordering.API.md file.
5. Observe the content of the file. You can open the file preview by pressing `Ctrl-Shift-V`.
6. Make modification to the markdown file by using the following prompt:

   ```plaintext
   add dependencies
   ```

7. GitHub Copilot confirms the plan and adds dependencies under Technical Overview.
8. Click **Accept** button and save the file.

**Additional Notes** It is important to review and ensure documentation generated is accurate and made according to the responsible AI principles. e.g. [Responsible AI with GitHub Copilot](https://learn.microsoft.com/en-us/training/modules/responsible-ai-with-github-copilot/). Perform due diligence when generating documentation using Copilot and always perform peer review before publishing content.

## API Documentation

In this step, we will use Copilot Edits to generate API documentation for `api/orders` resource.

1. In Visual Studio Code, navigate to the `samples/eshop/src/Ordering.API` folder.
2. Open the Copilot Edits pane in Visual Studio Code by clicking the Copilot icon in the sidebar or using the command palette (`Ctrl+Shift+I`). In the chat settings, Lets select `o1-preview` model for API documentation.
3. Open `Orders.Api.cs` and `Ordering.API.csproj` files.
Generate a readme markdown file for the project by entering the following prompt:

   ```plaintext
   Generate API documentation for api/orders resource using OpenAPI Specification in YAML format and save it in Ordering.API.YAML file
   ```

4. GitHub Copilot confirms the plan and creates requested documentation to the Ordering.API.md file.
5. Observe the content of the file.
6. Sometimes copilot generates more methods and other times it might not generate all the methods for the API. Add the following prompt to explicitly specify the number of methods in the API:

   ```plaintext
   generate the documentation to only the 7 methods specified in `OrdersApi.cs`
   ```

7. Review and Confirm the documentation is created accurately.
8. Click **Accept** button and save the file.
9. Now lets generate the same content in JSON format. We can do so by by specifying the format and the file name as follows:

   ```plaintext
   Generate API documentation for the 7 methods in api/orders resource using OpenAPI Specification in JSON format and save it in Ordering.API.JSON file
   ```

10. Observe the content of the file.  

### Explore Further

Take your learning to the next level with these optional exercises:

1. Try asking Copilot Edits to generate API documentation in HTML.
2. Next ask copilot to generate sample code in multiple languages (C#, Java, Go, python, etc.) to consume these APIs. Make sure the code snippets are generated for all methods in the resource.

## Troubleshooting

- **Generated response is truncated or looks incomplete:** LLM models have finite set of tokens to use which includes current scope and previous prompts.  
  **Solution:** You may try selecting a different model, or try clearing previous context (if it is irrelevant to the current prompt). Use /clear command.

## Summary

In this lab, you explored how to use GitHub Copilot to create high-quality documentation at different levels. You started by generating code-level comments using Copilot Chat and refining them with Copilot Edits to improve clarity and accuracy. Next, you learned to produce comprehensive project documentation using the **Ordering.API** project, capturing its functionality and architecture. Finally, you focused on writing API documentation for the `api/orders` resource by creating OpenAPI specifications in YAML and JSON formats and integrating them into the project.  

This lab provided hands-on experience in leveraging GitHub Copilot to streamline the documentation process, making it easier to document code, projects, and APIs efficiently and effectively.

**Next Steps:**

- [Lesson 5: DevOps with GitHub Copilot](5-devops-with-copilot.md)
