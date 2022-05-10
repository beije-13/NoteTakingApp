using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

namespace NoteTakingApp
{

    public class RichTextBoxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();

        public static string GetDocumentRTF(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentRTFProperty);
        }

        public static void SetDocumentRTF(DependencyObject obj, MemoryStream stream)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            StreamReader reader = new StreamReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            string value = reader.ReadToEnd();
            obj.SetValue(DocumentRTFProperty, value);
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentRTFProperty = DependencyProperty.RegisterAttached(
            "DocumentRTF",
            typeof(string),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (obj, e) =>
                {
                    if (_recursionProtection.Contains(Thread.CurrentThread))
                        return;
                    /** 
                        assume we have RTF string in DocumentRTF property
                        which is bound to SelectedNote.Document, so, basically, the contents are the same

                        So we need to load DocumentRTF(RTF string) into obj(RichTextBox).Document when we change SelectedNote.Document
                        And we need to save obj(RichTextBox).Document into DocumentRTF(RTF string) when the obj.TextChanged is raised

                        FOR LOADING RTF INTO Document:
                            var content = new TextRange(obj.Document.ContentStart, obj.Document.ContentEnd);
                            if (content.CanLoad(DataFormats.Rtf))
                            {
                                content.Load(DocumentRTF, DataFormats.Rtf);
                            }
                        
                        FOR SAVING Document INTO RTF
                            var content = new TextRange(obj.Document.ContentStart, obj.Document.ContentEnd);
                            if (content.CanSave(DataFormats.Rtf))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    content.Save(stream, DataFormats.Rtf);
                                }
                            }
                     **/

                    var richTextBox = (RichTextBox)obj;

                    // LOADING RTF INTO Document
                    try
                    {
                        var content = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        if (content.CanLoad(DataFormats.Rtf))
                        {
                            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(GetDocumentRTF(richTextBox))))
                            {
                                content.Load(stream, DataFormats.Rtf);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        richTextBox.Document = new FlowDocument();
                    }

                    // SAVING Document INTO RTF
                    richTextBox.TextChanged += (obj2, e2) =>
                    {
                        RichTextBox richTextBox2 = obj2 as RichTextBox;
                        if (richTextBox2 != null)
                        {
                            var content = new TextRange(richTextBox2.Document.ContentStart, richTextBox2.Document.ContentEnd);
                            if (content.CanSave(DataFormats.Rtf))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    content.Save(stream, DataFormats.Rtf);
                                    SetDocumentRTF(richTextBox, stream);
                                }
                            }
                        }
                    };
                }
            )
        );
    }
}