# hellowcopilot

## Instructions

### Setup

1. Clone the repository:
    ```sh
    git clone https://github.com/mjd-v/hellowcopilot.git
    cd hellowcopilot
    ```

2. Set up the Google Sheets API:
    - Follow the instructions at https://developers.google.com/sheets/api/quickstart/dotnet to create a project and enable the Google Sheets API.
    - Download the `credentials.json` file and place it in the root directory of the project.

3. Set up the OpenAI API:
    - Sign up for an API key at https://beta.openai.com/signup/.
    - Store the API key in a secure location.

4. Set up the LinkedIn API:
    - Follow the instructions at https://docs.microsoft.com/en-us/linkedin/shared/authentication/authorization-code-flow to create a LinkedIn app and get your client ID, client secret, and redirect URI.
    - Obtain an access token using the authorization code flow.
    - Store the access token in a secure location.

### Running the Program

1. Build the project:
    ```sh
    dotnet build
    ```

2. Run the program:
    ```sh
    dotnet run
    ```

3. The program will open the Google Sheet, process the rows, make API calls to OpenAI, and store the returned data in the second column of each row. Debug information will be printed to the console.

4. To run the LinkedIn integration program:
    ```sh
    dotnet run --project linkedinapi.csproj
    ```

5. The LinkedIn integration program will fetch all your LinkedIn messages and reply to each one with a thank you message.
