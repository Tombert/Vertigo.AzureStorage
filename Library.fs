namespace Vertigo

module AzureStorage =
    open FSharp.Control
    open Microsoft.Framework.Configuration;
    open Microsoft.WindowsAzure.Storage;
    open Microsoft.WindowsAzure.Storage.Queue;

    module AzureQueue = 

        type CloudQueue with  
            member this.AsyncAddMessage(c: CloudQueueMessage) = 
                this.AddMessageAsync c
                |> Async.AwaitTask

            member this.DeleteMessage(c: CloudQueueMessage) = 
                c
                |> this.DeleteMessageAsync 
                |> Async.AwaitTask
                |> Async.RunSynchronously

            member this.AsyncDeleteMessage (c: CloudQueueMessage) =
                c
                |> this.DeleteMessageAsync
                |> Async.AwaitTask


        let fromAccountStr acc topic = 
            let storageAccount = CloudStorageAccount.Parse(acc)
            let queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.GetQueueReference(topic);

        let toAsyncSeq (queue: CloudQueue) (chunkSize: int) (pollrate: System.TimeSpan) = asyncSeq {
            while true do
                let! messages = queue.GetMessagesAsync(chunkSize) |> Async.AwaitTask
                yield messages
                do! Async.Sleep(pollrate.Milliseconds)
         }
