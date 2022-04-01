using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using PresentationRobot.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

// Add
using PresentationRobot.Enums;
using System.ComponentModel;
using System.Windows.Forms; // Form アセンブリ参照の追加が必要

namespace PresentationRobot.Activities
{
    [LocalizedDisplayName(nameof(Resources.DisplayMessage_DisplayName))]
    [LocalizedDescription(nameof(Resources.DisplayMessage_Description))]
    public class DisplayMessage : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_Message_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_Message_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [DefaultValue("ロボット実行中")]
        public InArgument<string> Message { get; set; } = "ロボット実行中";

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_FontSize_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_FontSize_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [DefaultValue(50)]
        public InArgument<int> FontSize { get; set; } = 50;

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_TextColor_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_TextColor_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [DefaultValue(typeof(Color), "White")]
        public InArgument<Color> TextColor { get; set; } = Color.White;

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_BackgroundColor_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_BackgroundColor_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [DefaultValue(typeof(Color), "Black")]
        public InArgument<Color> BackgroundColor { get; set; } = Color.Black;

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_BackgroundOpacity_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_BackgroundOpacity_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        [DefaultValue(0.9)]
        public InArgument<Double> BackgroundOpacity { get; set; } = 0.9;

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_Image_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_Image_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public ImageType Image { get; set; }

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_Position_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_Position_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public PositionType Position { get; set; }

        [LocalizedDisplayName(nameof(Resources.DisplayMessage_IsFullWidth_DisplayName))]
        [LocalizedDescription(nameof(Resources.DisplayMessage_IsFullWidth_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public Boolean IsFullWidth { get; set; }

        #endregion


        #region Constructors

        public DisplayMessage()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Message == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Message)));
            if (FontSize == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FontSize)));
            if (TextColor == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TextColor)));
            if (BackgroundColor == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(BackgroundColor)));
            if (BackgroundOpacity == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(BackgroundOpacity)));
            if (Image == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Image)));
            if (Position == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Position)));
            if (IsFullWidth == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(IsFullWidth)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var message = Message.Get(context);
            var fontsize = FontSize.Get(context);
            var textcolor = TextColor.Get(context);
            var backgroundcolor = BackgroundColor.Get(context);
            var backgroundopacity = BackgroundOpacity.Get(context);
            //var image = Image.Get(context);
            //var position = Position.Get(context);
            //var isfullwidth = IsFullWidth.Get(context);
            var position = Position;
            var image = Image;
            var isfullwidth = IsFullWidth;

            ///////////////////////////
            // Add execution logic HERE
            ///////////////////////////

            // Add From --->

            var label = new Label
            {
                Font = new Font("Arial", fontsize, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = message,
                AutoSize = true,
                ForeColor = textcolor
            };

            var pictureBox = new PictureBox
            {
                Width = label.PreferredHeight,
                Height = label.PreferredHeight
            };

            label.Left = pictureBox.Width + 10;
            label.Top = 10;
            pictureBox.Left = 10;

            //画像を表示する
            switch (image)
            {
                default:
                case ImageType.FlyingRobot:
                    pictureBox.Image = Properties.Resources.Flying_Robot;
                    break;
                case ImageType.ListeningRobot:
                    pictureBox.Image = Properties.Resources.Listening_Robot;
                    break;
                case ImageType.ProcessingRobot:
                    pictureBox.Image = Properties.Resources.Processing_Robot;
                    break;
                case ImageType.RecorderRobot:
                    pictureBox.Image = Properties.Resources.Recorder_Robot;
                    break;
                case ImageType.SearchingRobot:
                    pictureBox.Image = Properties.Resources.Searching_Robot;
                    break;
                case ImageType.ReceivingRobot:
                    pictureBox.Image = Properties.Resources.Receiving_Robot;
                    break;
            }

            //画像の大きさをPictureBoxに合わせる
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            var form = new Form
            {
                Width = pictureBox.Left + pictureBox.Width + label.PreferredWidth + 20,
                Height = label.PreferredHeight + 10,
                BackColor = backgroundcolor,
                Opacity = backgroundopacity,
                FormBorderStyle = FormBorderStyle.None,
                TopMost = true
            };

            form.Show();

            switch (position)
            {
                case PositionType.DownLeft:
                    form.Left = 0;
                    form.Top = Screen.PrimaryScreen.WorkingArea.Height - form.Height;
                    break;
                case PositionType.DownRight:
                    form.Left = Screen.PrimaryScreen.WorkingArea.Width - form.Width;
                    form.Top = Screen.PrimaryScreen.WorkingArea.Height - form.Height;
                    break;
                case PositionType.TopLeft:
                    form.Left = 0;
                    form.Top = 0;
                    break;
                case PositionType.TopRight:
                    form.Left = Screen.PrimaryScreen.WorkingArea.Width - form.Width;
                    form.Top = 0;
                    break;
            }
            
            if (isfullwidth)
            {
                form.Left = 0;
                form.Width = Screen.PrimaryScreen.WorkingArea.Width;
            }

            form.Controls.Add(pictureBox);
            form.Controls.Add(label);
            form.Update();

            // デバッグ時、有効にする
            //System.Threading.Thread.Sleep(3000);
            //form.Close();

            // Add To <---

            // Outputs
            return (ctx) => {
            };
        }

        #endregion
    }
}

