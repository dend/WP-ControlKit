using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.ObjectModel;

namespace ControlKit
{
    public class ZuneCard : ContentControl
    {
        private const string SourceURL = "http://socialapi.zune.net/en-US/members/{0}";
        private const string AvatarURL = "http://cache-tiles.zune.net/tiles/user/{0}";
        private const string BackgroundURL = "http://cache-tiles.zune.net/tiles/background/{0}";
        private const string BadgeURL = "http://socialapi.zune.net/en-US/members/{0}/badges";
        private const string RecentTracksURL = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-RecentTracks";
        private const string AlbumURL = "http://catalog.zune.net/v3.2/en-US/music/album/{0}/";
        private const string PictureURL = "http://image.catalog.zune.net/v3.2/en-US/image/{0}?width=64&height=64";

        public ZuneCard()
        {
            DefaultStyleKey = typeof(ZuneCard);
            this.Loaded += new RoutedEventHandler(ZuneCard_Loaded);
            RecentTracks = new ObservableCollection<ImageSource>();
        }

        void ZuneCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                DownloadData(DownloadType.BasicUserInfo);

                BitmapImage bSource = new BitmapImage(new Uri(string.Format(AvatarURL, UserID)));
                AvatarImageSource = bSource ?? new BitmapImage(new Uri("Graphics/64x64_tile.jpg", UriKind.Relative));

                bSource = new BitmapImage(new Uri(string.Format(BackgroundURL, UserID)));
                BackgroundImageSource = bSource ?? new BitmapImage(new Uri("Graphics/default_bg.jpg", UriKind.Relative));
            }
        }

        private void DownloadData(DownloadType downloadType, string parameter = "")
        {
            var client = new WebClient();
            XDocument document;

            if (downloadType == DownloadType.BasicUserInfo)
            {
                client.DownloadStringAsync(new Uri(string.Format(SourceURL, UserID)));
                client.DownloadStringCompleted += (s, e) =>
                    {
                        try
                        {
                            document = XDocument.Parse(e.Result);
                            PlayCount = document.Root.Element("{http://schemas.zune.net/profiles/2008/01}playcount").Value.ToString();

                            DownloadData(DownloadType.Badges);
                        }
                        catch
                        {
                            PlayCount = "0";
                        }
                    };
            }
            else if (downloadType == DownloadType.Badges)
            {
                client.DownloadStringAsync(new Uri(string.Format(BadgeURL, UserID)));
                client.DownloadStringCompleted += (s, e) =>
                    {
                        try
                        {
                            document = XDocument.Parse(e.Result);
                            BadgeCount = (from c in document.Root.Elements()
                                          where c.Name == "{http://www.w3.org/2005/Atom}entry"
                                          select c).Count().ToString();

                            DownloadData(DownloadType.RecentTracks);
                        }
                        catch
                        {
                            BadgeCount = "0";
                        }
                    };
            }
            else if (downloadType == DownloadType.RecentTracks)
            {
                client.DownloadStringAsync(new Uri(string.Format(RecentTracksURL, UserID)));
                client.DownloadStringCompleted += (s, e) =>
                {
                    try
                    {
                        document = XDocument.Parse(e.Result);
                        var selection = (from c in document.Root.Elements() where c.Name == "{http://www.w3.org/2005/Atom}entry" select c).Take(4);

                        foreach (XElement element in selection)
                        {
                            string albumID =  element.Element("{http://schemas.zune.net/catalog/music/2007/10}track")
                                .Element("{http://schemas.zune.net/catalog/music/2007/10}album")
                                .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Replace("urn:uuid:","");

                           DownloadData(DownloadType.AlbumImage, albumID);
                        }
                    }
                    catch
                    {
                        
                    }
                };
            }
            else if (downloadType == DownloadType.AlbumImage)
            {
                client.DownloadStringAsync(new Uri(string.Format(AlbumURL, parameter)));
                client.DownloadStringCompleted += (s, e) =>
                {
                    try
                    {
                        document = XDocument.Parse(e.Result);
                        string imageID = document.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}image")
                            .Value.Replace("urn:uuid:", "");

                        RecentTracks.Add(new BitmapImage(new Uri(string.Format(PictureURL, imageID))));
                    }
                    catch
                    {
                        
                    }
                };
            }
        }

        public static readonly DependencyProperty RecentTracksProperty =
         DependencyProperty.Register("RecentTracks", typeof(ObservableCollection<ImageSource>), typeof(ZuneCard), new PropertyMetadata(new ObservableCollection<ImageSource>()));

        public ObservableCollection<ImageSource> RecentTracks
        {
            get 
            {
                return GetValue(RecentTracksProperty) as ObservableCollection<ImageSource>; 
            }
            set 
            {
                SetValue(RecentTracksProperty, value);
            }
        }

        public static readonly DependencyProperty UserIDProperty =
         DependencyProperty.Register("UserID", typeof(string), typeof(ZuneCard), new PropertyMetadata(string.Empty));

        public string UserID
        {
            get { return GetValue(UserIDProperty) as string; }
            set { SetValue(UserIDProperty, value); }
        }

        public static readonly DependencyProperty BadgeCountProperty =
            DependencyProperty.Register("BadgeCount", typeof(string), typeof(ZuneCard), new PropertyMetadata("0"));

        public string BadgeCount
        {
            get { return (string)GetValue(BadgeCountProperty); }
            set { SetValue(BadgeCountProperty, value); }
        }

        public static readonly DependencyProperty PlayCountProperty =
            DependencyProperty.Register("PlayCount", typeof(string), typeof(ZuneCard), new PropertyMetadata("0"));

        public string PlayCount
        {
            get { return (string)GetValue(PlayCountProperty); }
            set { SetValue(PlayCountProperty, value); }
        }

        public static readonly DependencyProperty AvatarImageSourceProperty =
            DependencyProperty.Register("AvatarImageSource", typeof(ImageSource), typeof(ZuneCard), new PropertyMetadata(null));

        public ImageSource AvatarImageSource
        {
            get { return (ImageSource)GetValue(AvatarImageSourceProperty); }
            set { SetValue(AvatarImageSourceProperty, value); }
        }

        public static readonly DependencyProperty BackgroundImageSourceProperty =
            DependencyProperty.Register("BackgroundImageSource", typeof(ImageSource), typeof(ZuneCard), new PropertyMetadata(null));

        public ImageSource BackgroundImageSource
        {
            get { return (ImageSource)GetValue(BackgroundImageSourceProperty); }
            set { SetValue(BackgroundImageSourceProperty, value); }
        }
    }

    public enum DownloadType
    {
        BasicUserInfo,
        Badges,
        RecentTracks,
        AlbumImage
    }
}
