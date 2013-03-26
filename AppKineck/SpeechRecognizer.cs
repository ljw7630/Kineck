using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace AppKineck
{
	class SpeechRecognizer
	{
		private const string MediumGreyBrushKey = "MediumGreyBrush";
		private SpeechRecognitionEngine speechEngine;
		private List<Span> recognitionSpans;

		private MainWindow window = (MainWindow) App.Current.MainWindow;

		public SpeechRecognizer()
		{
			RecognizerInfo ri = GetKinectRecognizer();

			if (null != ri)
			{
				recognitionSpans = new List<Span> { window.upSpan, window.downSpan, window.rightSpan, window.leftSpan };

				this.speechEngine = new SpeechRecognitionEngine(ri.Id);

				/****************************************************************
				* 
				* Use this code to create grammar programmatically rather than from
				* a grammar file.
				* 
				* var directions = new Choices();
				* directions.Add(new SemanticResultValue("forward", "FORWARD"));
				* directions.Add(new SemanticResultValue("forwards", "FORWARD"));
				* directions.Add(new SemanticResultValue("straight", "FORWARD"));
				* directions.Add(new SemanticResultValue("backward", "BACKWARD"));
				* directions.Add(new SemanticResultValue("backwards", "BACKWARD"));
				* directions.Add(new SemanticResultValue("back", "BACKWARD"));
				* directions.Add(new SemanticResultValue("turn left", "LEFT"));
				* directions.Add(new SemanticResultValue("turn right", "RIGHT"));
				*
				* var gb = new GrammarBuilder { Culture = ri.Culture };
				* gb.Append(directions);
				*
				* var g = new Grammar(gb);
				* 
				****************************************************************/

				// Create a grammar from grammar definition XML file.
				using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
				{
					var g = new Grammar(memoryStream);
					speechEngine.LoadGrammar(g);
				}

				speechEngine.SpeechRecognized += SpeechRecognized;
				speechEngine.SpeechRecognitionRejected += SpeechRejected;

				speechEngine.SetInputToAudioStream(
				window.sensorChooser.Kinect.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
				speechEngine.RecognizeAsync(RecognizeMode.Multiple);
			}
			else
			{
				window.statusBarText.Text = Properties.Resources.NoSpeechRecognizer;
			}
		}

		private static RecognizerInfo GetKinectRecognizer()
		{
			foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
			{
				string value;
				recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
				if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
				{
					return recognizer;
				}
			}

			return null;
		}

		private void ClearRecognitionHighlights()
		{
			foreach (Span span in recognitionSpans)
			{
				span.Foreground = (Brush)window.Resources[MediumGreyBrushKey];
				span.FontWeight = FontWeights.Normal;
			}
		}

		private void Dispose()
		{
			if (null != this.speechEngine)
			{
				this.speechEngine.SpeechRecognized -= SpeechRecognized;
				this.speechEngine.SpeechRecognitionRejected -= SpeechRejected;
				this.speechEngine.RecognizeAsyncStop();
			}
		}

		private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			// Speech utterance confidence below which we treat speech as if it hadn't been heard
			const double confidenceThreshold = 0.3;

			ClearRecognitionHighlights();

			if (e.Result.Confidence >= confidenceThreshold)
			{
				switch (e.Result.Semantics.Value.ToString())
				{
					case "LEFT":
						window.leftSpan.Foreground = Brushes.DeepSkyBlue;
						window.leftSpan.FontWeight = FontWeights.Bold;
						break;

					case "RIGHT":
						window.rightSpan.Foreground = Brushes.DeepSkyBlue;
						window.rightSpan.FontWeight = FontWeights.Bold;
						break;

					case "UP":
						window.upSpan.Foreground = Brushes.DeepSkyBlue;
						window.upSpan.FontWeight = FontWeights.Bold;
						break;

					case "DOWN":
						window.downSpan.Foreground = Brushes.DeepSkyBlue;
						window.downSpan.FontWeight = FontWeights.Bold;
						break;

					case "MUTE":
						window.muteSpan.Foreground = Brushes.DeepSkyBlue;
						window.muteSpan.FontWeight = FontWeights.Bold;
						break;
				}
				window.Sender.AnalyzeAndSend(e.Result.Semantics.Value.ToString());
			}
		}

		private void SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
		{
			ClearRecognitionHighlights();
		}
	}
}
