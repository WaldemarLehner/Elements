namespace ComputergrafikSpiel.Model.Interfaces
{
    // dtime => time between 2 Frames in OnUpdateFrame
    internal interface IUpdateable
    {
        void Update(float dtime);
    }
}