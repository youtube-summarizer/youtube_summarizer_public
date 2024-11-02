# YouTubeSummarizer

YouTubeSummarizer is a web application that processes YouTube videos, extracts their captions, and generates summaries using ChatGPT. This project leverages C# and Angular, with Python for the summarization microservice. 
Amazon SQS is used to queue video summarization requests, and the generated summaries are stored in a database.

## Features

- **Video Analysis**: Extracts and processes captions from YouTube videos.
- **Summarization**: Utilizes ChatGPT to create concise and accurate summaries.
- **Queue Management**: Manages video summarization requests with Amazon SQS.
- **Database Storage**: Stores generated summaries in a database.

## Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Architecture](#architecture)
- [Links](#links)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

To set up a local copy of the project, follow these steps:

### Prerequisites

Ensure you have the following installed:

- .NET SDK 6.x or later
- Node.js and npm
- Python 3.x
- AWS CLI
- Docker (optional, for database setup)

### Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/youtube-summarizer/youtube-summarizer.git
    cd youtubesummarizer
    ```

2. **Install .NET and Node.js dependencies**:
    - Navigate to the C# backend folder and restore dependencies:
        ```bash
        cd backend
        dotnet restore
        ```
    - Navigate to the Angular frontend folder and install dependencies:
        ```bash
        cd ../frontend
        npm install
        ```

3. **Install Python dependencies** for the summarizer microservice:
    ```bash
    cd ../summarizer
    pip install -r requirements.txt
    ```

4. **Set up the database** (optional, using Docker):
    ```bash
    docker-compose up -d
    ```

### Configuration

1. **AWS Configuration**:
    Ensure your AWS CLI is configured with the necessary access keys and region.

    ```bash
    aws configure
    ```

2. **Environment Variables**:
    Create `.env` files in each directory and add the following configurations:

    - **Backend `.env`**:
        ```plaintext
        AWS_ACCESS_KEY_ID=your_aws_access_key
        AWS_SECRET_ACCESS_KEY=your_aws_secret_key
        AWS_REGION=your_aws_region
        SQS_QUEUE_URL=your_sqs_queue_url
        DATABASE_URL=your_database_connection_string
        ```

    - **Summarizer `.env`**:
        ```plaintext
        OPENAI_API_KEY=your_openai_api_key
        ```

## Usage

1. **Start the Backend and Frontend**:
    - Navigate to the backend directory and run the application:
        ```bash
        cd ../backend
        dotnet run
        ```
    - Navigate to the frontend directory and start the Angular development server:
        ```bash
        cd ../frontend
        npm start
        ```

2. **Submit a Video for Summarization**:
    Use the web interface to submit a YouTube video URL. The backend will handle the video processing, send the request to Amazon SQS, and the summarizer microservice will generate and store the summary.

3. **Retrieve Summaries**:
    Summaries can be viewed and retrieved from the web interface or the database.

## Architecture

1. **Frontend**:
    - An Angular-based web interface for submitting video URLs and displaying summaries.

2. **Backend**:
    - A C# ASP.NET Core API that handles video submissions, interacts with the database, and manages SQS queue requests.

3. **Summarizer Microservice**:
    - A Python service that processes video captions and generates summaries using ChatGPT.

4. **Queue Management**:
    - Amazon SQS is used to manage and queue video summarization tasks.

5. **Database**:
    - A relational database (e.g., PostgreSQL) stores video summaries.

### Database Schema:
![Database Schema](https://github.com/youtube-summarizer/youtube_summarizer_public/blob/main/assets/database.png)

### Workflow: 
![Workflow](https://github.com/youtube-summarizer/youtube_summarizer_public/blob/main/assets/workflow.png)

### Links:
[Youtube Presentation](https://www.youtube.com/watch?v=F0XFRyNxHZk)


### Contributing

Contributions are welcome! Here’s how to contribute:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

Project Link: [https://github.com/youtube-summarizer/youtube-summarizer](https://github.com/youtube-summarizer/youtube-summarizer)

---

This README provides a detailed guide for setting up and using the YouTubeSummarizer project. For any issues or questions, feel free to reach out through the contact information. Happy summarizing!

## Disclaimer

The keys and secrets that were initially used for AWS, the OpenAI API and the Youtube API have been removed from the repo.
