using System.Reflection.Metadata;
using System.Threading.Tasks;
using backend.Application.Common.Interfaces;

public static class JobExtensions
{

    public static async Task<IList<MediaProcessingJob>> GetAllParents(this MediaProcessingJob job, IApplicationDbContext dbContext, CancellationToken token = default)
    {
        var jobs = new List<MediaProcessingJob>();

        await dbContext.MediaProcessingJobs.Entry(job).Reference(j => j.ParentJob).LoadAsync(token);

        if (job.ParentJob is not null)
        {
            if (job.ParentJob is IHasInputResource parentJobWithInput)
            {
                await dbContext.Entry(parentJobWithInput).Reference(r => r.InputResource).LoadAsync(token);
            }

            if (job.ParentJob is IHasOutputResource parentJobWithOutput)
            {
                await dbContext.Entry(parentJobWithOutput).Reference(r => r.OutputResource).LoadAsync(token);
            }

            jobs.Add(job.ParentJob);
            var parentJobs = await job.ParentJob.GetAllParents(dbContext, token);
            jobs.AddRange(parentJobs);
        }

        return jobs;
    }
    public static async Task<T?> GetJobOfType<T>(this MediaProcessingJob job, IApplicationDbContext dbContext, CancellationToken token = default) where T : MediaProcessingJob
    {
        var jobs = await job.GetAllParents(dbContext, token);
        return jobs.OfType<T>().FirstOrDefault();
    }
}