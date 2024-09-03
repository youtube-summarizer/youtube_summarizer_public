﻿using Amazon.SQS;
using Amazon.SQS.Model;

namespace YtbSummarizer.CheckerWorker.Services;

public class SqsService
{
	private readonly string _queueUrl;
	private readonly IAmazonSQS _sqsClient;

	public SqsService(IConfiguration configuration)
	{
		_queueUrl = configuration["AWS:QueueUrl"]!;
		_sqsClient = new AmazonSQSClient();
	}

	public async Task SendMessageAsync(string messageBody)
	{
		var sendMessageRequest = new SendMessageRequest
		{
			QueueUrl = _queueUrl,
			MessageBody = messageBody
		};

		await _sqsClient.SendMessageAsync(sendMessageRequest);
	}
}