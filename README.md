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
