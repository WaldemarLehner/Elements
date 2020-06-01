namespace ComputergrafikSpiel.Model.Interfaces
{
    // dtime => time between 2 Frames in OnUpdateFrame
    public interface IUpdateable
    {
        void Update(float dtime);
    }
}