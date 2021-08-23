using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleBotLinguist
{
    public class Localization
    {
        public string Code { get; set; }
        public string Answer { get; set; }
        public string ErrorCommand { get; set; }
        public string Suggestion { get; set; }
        public string ITranslator { get; set; }
        public string IPredictor { get; set; }
        public string IOptions { get; set; }
        public string ErrorLang { get; set; }
        public string AllRight { get; set; }
        public string ISpeller { get; set; }
        public string Variants { get; set; }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            (new MainProgram()).StartProgram();
        }
    }

    class MainProgram
    {
        private BackgroundWorker bw;
        private Settings settings;
        private Dictionary<string, Localization> localization;
        private string translatorResponse;
        private string predictorResponse;
        private Telegram.Bot.Types.Message smessage;
        private string spellerResponse;

        public void StartProgram()
        {
            bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWorkAsync;

            #region FlagInitialization
            localization = new Dictionary<string, Localization>() {
                { "🇷🇺", new Localization() {
                    Code ="ru",
                    Answer ="Выбран русский язык!",
                    ErrorCommand = "Неверная команда!",
                    Suggestion = "Выберите язык!",
                    ITranslator = "Переводчик",
                    IPredictor = "Предсказатель",
                    IOptions = "Выберите вариант!",
                    ErrorLang = "Выбранный язык не поддерживается!",
                    AllRight = "Все верно!",
                    ISpeller = "Правописание",
                    Variants = "Варианты" }
                },
                { "🇬🇧", new Localization() {
                    Code ="en",
                    Answer ="The language is English!",
                    ErrorCommand = "Incorrect command",
                    Suggestion = "Choose your language!",
                    ITranslator = "Translator",
                    IPredictor = "Predictor",
                    IOptions = "Choose an option!",
                    ErrorLang = "The selected language is not supported!",
                    AllRight = "All right!",
                    ISpeller = "Speller",
                    Variants = "Suggestions" }
                },
                { "🇵🇱", new Localization() {
                    Code ="pl",
                    Answer ="Wybrany język polski!",
                    ErrorCommand = "Niepoprawne polecenie!",
                    Suggestion = "Wybierz język!",
                    ITranslator = "Tłumacz",
                    IPredictor = "Wróżbita",
                    IOptions = "Wybierz opcję!",
                    ErrorLang = "Wybrany język nie jest obsługiwany!",
                    AllRight = "Wszystko się zgadza!",
                    ISpeller = "Pisownia",
                    Variants = "Oferta" }
                },
                { "🇺🇦", new Localization() {
                    Code ="uk",
                    Answer ="Обрано українську мову!",
                    ErrorCommand = "Невірна команда!",
                    Suggestion = "Виберіть мову!",
                    ITranslator = "Перекладач",
                    IPredictor = "Провісник",
                    IOptions = "Виберіть варіант!",
                    ErrorLang = "Вибрана мова не підтримується!",
                    AllRight = "Все правильно!",
                    ISpeller = "Правопис",
                    Variants = "Пропозиція" }
                },
                { "🇩🇪", new Localization() {
                    Code ="de",
                    Answer ="Deutsch gewählt!",
                    ErrorCommand = "Falscher Befehl!",
                    Suggestion = "Wählen Sie eine Sprache!",
                    ITranslator = "Übersetzer",
                    IPredictor = "Predictor",
                    IOptions = "Wählen Sie eine option!",
                    ErrorLang = "Die ausgewählte Sprache wird nicht unterstützt!",
                    AllRight = "Alles klar!",
                    ISpeller = "Rechtschreibung",
                    Variants = "Vorschlag" }
                },
                { "🇫🇷", new Localization() {
                    Code ="fr",
                    Answer ="Le français est choisi!",
                    ErrorCommand = "Erreur sur la commande!",
                    Suggestion = "Choisissez votre langue!",
                    ITranslator = "Traducteur",
                    IPredictor = "Prédicteur",
                    IOptions = "Choisissez une option!",
                    ErrorLang = "La langue sélectionnée n'est pas pris en charge!",
                    AllRight = "Tout droit!",
                    ISpeller = "Orthographe",
                    Variants = "Suggestion" }
                },
                { "🇪🇸", new Localization() {
                    Code ="es",
                    Answer ="¡Seleccionado español!",
                    ErrorCommand = "Comando incorrecto!",
                    Suggestion = "Seleccione el idioma!",
                    ITranslator = "Traductor",
                    IPredictor = "Predictor",
                    IOptions = "Elija una opción!",
                    ErrorLang = "El idioma seleccionado no es compatible!",
                    AllRight = "¡Órale!",
                    ISpeller = "Ortografía",
                    Variants = "Sugerencia" }
                },
                { "🇮🇹", new Localization() {
                    Code ="it",
                    Answer ="Lingua italiana selezionata!",
                    ErrorCommand = "Comando errato!",
                    Suggestion = "Scegli la lingua!",
                    ITranslator = "Traduttore",
                    IPredictor = "Predictor",
                    IOptions = "Scegli un'opzione!",
                    ErrorLang = "La lingua selezionata non è supportata!",
                    AllRight = "Va bene!",
                    ISpeller = "Ortografia",
                    Variants = "Suggerimento" }
                },
                { "🇹🇷", new Localization() {
                    Code ="tr",
                    Answer ="Türk Dili seçildi!",
                    ErrorCommand = "Yanlış komut!",
                    Suggestion = "Bir dil seçin!",
                    ITranslator = "Çevirici",
                    IPredictor = "Tahmini",
                    IOptions = "Bir seçenek seçin!",
                    ErrorLang = "Seçilen dil desteklenmiyor!",
                    AllRight = "Pekala!",
                    ISpeller = "Yazım",
                    Variants = "Öneriler" }
                }
            };
            #endregion

            var text = "Telegram key";
            bw.RunWorkerAsync(text); //запускаем
            Console.WriteLine("Бот запущен...");

            if (Console.ReadLine() == "stop")
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Settings));

                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream("settings.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, settings);
                }

                return;
            }
        }

        async void Bw_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            settings = GetSettings();
            var worker = sender as BackgroundWorker; //получаем ссылку на класс вызвавший событие
            var key = e.Argument as string; //получаем ключ из аргументов
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key);
                await Bot.SetWebhookAsync(""); //убираем старую привязку к вебхуку для бота

                GetCallback(Bot);

                Bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
                {
                    if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return;
                    var update = evu.Update;
                    var message = update.Message;
                    if (message == null) return;

                    if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                    {
                        if (!settings.Language.ContainsKey(message.Chat.Id))
                            settings.Language.Add(message.Chat.Id, "🇬🇧");

                        if (message.Text[0] == '/')
                        {
                            await ChoiceCommand(Bot, message);
                        }
                        else if (IsMessageFlag(message.Text))
                        {
                            await ChoiceLanguage(settings.Language, Bot, message, localization);
                        }
                        else
                        {
                            smessage = message;
                            await IButtonsCommand(Bot, message);
                        }
                    }
                };

                Bot.StartReceiving(); //запускаем прием обновлений
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); //если ключь не подошел - пишем об этом в консоль отладки
            }
        }

        private Settings GetSettings()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));

            // десериализация
            using (FileStream fs = new FileStream("settings.xml", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0)
                {
                    return new Settings();
                }

                return (Settings)formatter.Deserialize(fs);
            }
        }

        private void GetCallback(Telegram.Bot.TelegramBotClient Bot)
        {
            Bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                var message = ev.CallbackQuery.Message;
                string response = "";
                Localization lang = localization[settings.Language[smessage.Chat.Id]];
                switch (ev.CallbackQuery.Data)
                {
                    case "callback1":
                        response =
                        translatorResponse = Translator.Translator.Translate(smessage.Text, lang);
                        break;

                    case "callback2":
                        response =
                        predictorResponse = Predictor.Predictor.Predict(smessage.Text, lang);
                        break;

                    case "callback3":
                        response =
                        spellerResponse = Speller.Speller.Spell(smessage.Text, lang);
                        break;
                }
                await Bot.SendTextMessageAsync(message.Chat.Id, response, replyToMessageId: smessage.MessageId);
                await Bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id);
            };
        }

        private async System.Threading.Tasks.Task ChoiceCommand(Telegram.Bot.TelegramBotClient Bot, Telegram.Bot.Types.Message message)
        {
            switch (message.Text)
            {
                case "/saysomething":
                    await Bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                    break;

                case "/getimage":
                    await Bot.SendPhotoAsync(message.Chat.Id, "https://avatars.mds.yandex.net/get-pdb/468882/e8b7386f-f66b-438c-ab2d-d62926fb1565/s1200", "Картинка!");
                    break;

                case "/start":
                    await ClangCommand(Bot, message);
                    break;

                default:
                    await Bot.SendTextMessageAsync(message.Chat.Id, localization[settings.Language[message.Chat.Id]].ErrorCommand, replyToMessageId: message.MessageId);
                    break;
            }
        }

        private bool IsMessageFlag(string text)
        {
            return localization.ContainsKey(text);
        }

        private static async System.Threading.Tasks.Task ChoiceLanguage(Dictionary<long, string> language,
                                                                       Telegram.Bot.TelegramBotClient Bot,
                                                                       Telegram.Bot.Types.Message message,
                                                                       Dictionary<string, Localization> flags)
        {
            language.Remove(message.Chat.Id);
            language.Add(message.Chat.Id, message.Text);
            await Bot.SendTextMessageAsync(message.Chat.Id, flags[message.Text].Answer, replyToMessageId: message.MessageId);
        }

        private async System.Threading.Tasks.Task IButtonsCommand(Telegram.Bot.TelegramBotClient Bot, Telegram.Bot.Types.Message message)
        {
            var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton[][]
            {
                new []
                {
                    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton{Text = localization[settings.Language[message.Chat.Id]].ITranslator, CallbackData = "callback1"},
                    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton{Text = localization[settings.Language[message.Chat.Id]].IPredictor, CallbackData = "callback2"},
                    new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton{Text = localization[settings.Language[message.Chat.Id]].ISpeller, CallbackData = "callback3"}
                },
            });
            await Bot.SendTextMessageAsync(message.Chat.Id, localization[settings.Language[message.Chat.Id]].IOptions, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyboard);
        }

        private async System.Threading.Tasks.Task ClangCommand(Telegram.Bot.TelegramBotClient Bot, Telegram.Bot.Types.Message message)
        {
            var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
            {
                Keyboard = new[] {
                    new[] {
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇷🇺"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇬🇧"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇵🇱"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇺🇦"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇩🇪"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇫🇷"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇪🇸"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇮🇹"),
                        new Telegram.Bot.Types.ReplyMarkups.KeyboardButton("🇹🇷")
                    },
                },
                ResizeKeyboard = true
            };

            await Bot.SendTextMessageAsync(message.Chat.Id, localization[settings.Language[message.Chat.Id]].Suggestion, Telegram.Bot.Types.Enums.ParseMode.Default, false, false, 0, keyboard);
        }
    }
}
