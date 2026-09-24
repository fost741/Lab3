
public class Instruments
{
    private static int count = 0;
    private string name;
    private Brand brand;
    private int keyCount;
    private int stringCount;
    private bool isElectric;
    private bool isConnected;
    private Family family;
    private bool isTuned;
    private int currentVolume;

    public Instruments()
    {
        count++;
        RegCode = $"REG-{count:D2}";
    }
    public string RegCode { get; private set; } = "REG_00"; //автовластивістьб індивідуальний код інструменту

    public string Name
    { get { return name; }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Instrument name cannot be null or empty");
            name = value.Trim();
        } 
    }

    public Brand Brand 
    {
        get { return brand; }
        set { if (!Enum.IsDefined(typeof(Brand), value))
                throw new ArgumentException("Invalid brand specified");
            brand = value; }
    }

    public int KeyCount
    {
        get { return keyCount; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Key count cannot be negative");
            keyCount = value;
        }
    }

    public int StringCount
    {
        get { return stringCount; }
        set
        {
            if (value < 0)
                throw new ArgumentException("String count cannot be negative");
            stringCount = value;
        }
    }

    public bool IsElectric
    {
        get { return isElectric; }
        set
        {
            isElectric = value;
        }
    }

    public bool IsConnected
    {
        get { return isConnected; }
        set
        {
            isConnected = value;
        }
    }

    public Family Family
    {
        get { return family; }
        set
        {
            if (!Enum.IsDefined(typeof(Family), value))
                throw new ArgumentException("Invalid family specified");
            family = value;
        }
    }

    //властивість з різним рівнем доступу для set та get
    public bool IsTuned
    {
        get { return isTuned; }
        private set
        {
            isTuned = value;
        }
    }

    public int CurrentVolume
    {
        get { return currentVolume; }
        set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Volume should be between 0 and 100");
            currentVolume = value;
        }
    }

    //обчислювальна властивість
    public string SoundType
    {
        get
        { 
            if (!IsElectric) return "Acoustic";
            if (CurrentVolume == 0 || CurrentVolume < 50) return "Silent"; 
            return CurrentVolume > 70 ? "Loud" : "Normal";
        }
    }

    //приватний метод для приховування деталей реалізації паблік методів
    private bool CanTune(out string message)
    {
        if (isElectric && !isConnected)
        {
            message = $"Can not tune [{RegCode}]. Instrument {name} is not connected to power.";
            return false;
        }
        string instName = name.ToLower();
        if (family == Family.Strings)
        {
            if ((instName.Contains("ukulele") || instName.Contains("violin")) && stringCount != 4)
            {
                message = $"Instrument {name} [{RegCode}] can not be tuned: string amount is not 4. Your input: {stringCount}";
                return false;
            }
            else if ((instName.Contains("guitar") && stringCount != 6 && stringCount != 7 && stringCount != 12))
            {
                message = $"Instrument {name} [{RegCode}]can not be tuned: string amount should be 6 / 7 / 12. Your input: {stringCount}";
                return false;
            }
            else if (stringCount < 4)
            {
                message = $"Instrument {name} [{RegCode}] can not be tuned: string amount can not be < 4. Your input: {stringCount}";
                return false;
            }
        }
        if (family == Family.Keyboard)
        {
            if (!isElectric && keyCount != 88)
            {
                message = $"Instrument {name} [{RegCode}] can not be tuned: key amount is not 88. Your input: {keyCount}";
                return false;
            }
            else if (keyCount != 61 && keyCount != 76 && keyCount != 88)
            {
                message = $"Instrument {name} [{RegCode}] can not be tuned: key amount should be (61 / 76 / 88). Your input: {keyCount}";
                return false;
            }
        }
        message = $"{name} [{RegCode}] was successfuly tuned.";
        return true;
    }

    //загальнодоступні методи
    public string Tune() //настроювання інструменту
    {
        if (!CanTune(out string message))
        {
            IsTuned = false;
            return message;
        }

        IsTuned = true;
        return $"{name} was successfuly tuned.";
    }
    public string Play() 
    {
        if (!IsTuned) return $"Can not play {name}. The instrument [{RegCode}] is not tuned.";
        if (IsElectric) return $"Instrument {name} [{RegCode}] is playing at {CurrentVolume}% volume.";
        else return $"Instrument {name} [{RegCode}] is playing.";

    }
    public string Connect() //підключення інструменту до електроживлення
    {
        if (!isElectric) return $"Instrument {name} [{RegCode}] is not electric and does not require power connection.";

        isConnected = true;
        return $"Instrument {name} [{RegCode}] was successfuly connected to power.";
    }

    public string SetVolume(int volume) //встановлення гучності інструменту
    {
        if (!isElectric) return $"Can not set volume for this instrument. Instrument {name} [{RegCode}] is acoustic instrument.";
        if (volume < 0 || volume > 100) return $"Can not set this volume. Volume should be between 0 and 100.";
        currentVolume = volume;
        return $"Volume for instrument {name} [{RegCode}] is set to {currentVolume}%.";
    }

}

