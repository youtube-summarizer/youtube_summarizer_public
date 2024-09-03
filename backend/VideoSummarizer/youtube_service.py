import json
import os
import time

import openai
import requests
from youtube_transcript_api import YouTubeTranscriptApi

OPENAI_API_KEY = os.getenv('OPENAI_API_KEY')
ASSISTANT_ID = os.getenv('ASSISTANT_ID')
API_URL = os.getenv('API_URL')
API_KEY = os.getenv('API_KEY')


client = openai.OpenAI(api_key=OPENAI_API_KEY)


def submit_message(assistant_id, thread, user_message):
    """Create a Message on a Thread, then start (and return) a new Run."""
    client.beta.threads.messages.create(
        thread_id=thread.id, role="user", content=user_message
    )
    return client.beta.threads.runs.create(
        thread_id=thread.id,
        assistant_id=assistant_id,
    )


def get_response(thread):
    """Return the list of Messages in a Thread."""
    return client.beta.threads.messages.list(thread_id=thread.id, order="asc")


def create_thread_and_run(user_input):
    """Create a thread and submit a user message to it."""
    thread = client.beta.threads.create()
    run = submit_message(ASSISTANT_ID, thread, user_input)
    return thread, run


def wait_on_run(run, thread):
    """Wait for a run to complete and return the final run."""
    while run.status == "queued" or run.status == "in_progress":
        run = client.beta.threads.runs.retrieve(
            thread_id=thread.id,
            run_id=run.id,
        )
        time.sleep(0.5)
    return run


def pretty_print(messages):
    """Pretty print the messages."""
    print(messages.data[1].content[0].text.value)
    return messages.data[1].content[0].text.value


def get_captions(video_id):
    srt = YouTubeTranscriptApi.get_transcript(video_id, languages=['en'])
    all_captions = ""
    for caption in srt:
        all_captions += caption['text'] + " "

    return all_captions


# if __name__ == '__main__':
#     videoId = 'RyRo8eVsrlU'

#     captions = get_captions(videoId)

#     thread, run = create_thread_and_run(captions)
#     run = wait_on_run(run, thread)
#     returned_summary = pretty_print(get_response(thread))

#     print(captions)
#     print(returned_summary)


def lambda_handler(event, context):
    # Iterate over each record
    for record in event['Records']:
        # Parse the message body
        video_id = record["body"]
        print(f"Received message: {video_id}")

        # video_id = 'tPGsyteT-W0'

        captions = get_captions(video_id)

        thread, run = create_thread_and_run(captions)
        run = wait_on_run(run, thread)
        returned_summary = pretty_print(get_response(thread))

        headers = {
            "X-Api-Key": f"{API_KEY}",
            "Content-Type": "application/json",
        }

        payload = {
            "videoId": f"{video_id}",
            "summary": f"{returned_summary}",
        }

        response = requests.patch(
            API_URL, headers=headers, data=json.dumps(payload), verify=False)
        print("Status Code:", response.status_code)

    return {
        'statusCode': 200,
        'body': json.dumps('Message processed successfully')
    }
