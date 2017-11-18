namespace Vertigo

module AzureStorage =
    open FSharp.Control
    open Microsoft.Framework.Configuration;
    open Microsoft.WindowsAzure.Storage;
    open Microsoft.WindowsAzure.Storage.Queue;

    module AzureQueue = 
        let fromAccountStr acc topic = 
            let storageAccount = CloudStorageAccount.Parse(acc)
            let queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.GetQueueReference("messageQueue");
        let toAsyncSeq (queue: CloudQueue) (chunkSize: int) (pollrate: System.TimeSpan) = asyncSeq {
            while true do
                let! messages = queue.GetMessagesAsync(chunkSize) |> Async.AwaitTask
                yield messages
         }
