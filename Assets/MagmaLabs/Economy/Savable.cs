namespace MagmaLabs.Economy{
    public interface Savable
    {
        string ToString();
        void FromString(string serialized);
    }
}