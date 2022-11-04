namespace GitInsight.Core;

public interface IPersistentStorage{
    (Response, DbCommitDTO?) Find(string Filepath);

    Response Create(DbRepositoryCreateDTO dbRepositoryCreate);

    Response Update(DbRepositoryUpdateDTO dbRepositoryUpdate);
}