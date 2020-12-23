﻿using Flowframes.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowframes.AudioVideo
{
    class FFmpegUtils
    {
        public enum Codec { H264, H265, VP9, ProRes, AviRaw }


        public static Codec GetCodec(Interpolate.OutMode mode)
        {
            if (mode == Interpolate.OutMode.VidMp4)
            {
                bool h265 = Config.GetInt("mp4Enc") == 1;
                return h265 ? Codec.H265 : Codec.H264;
            }

            if (mode == Interpolate.OutMode.VidWebm)
                return Codec.VP9;

            if (mode == Interpolate.OutMode.VidProRes)
                return Codec.ProRes;

            if (mode == Interpolate.OutMode.VidAviRaw)
                return Codec.AviRaw;

            return Codec.H264;
        }

        public static string GetEnc(Codec codec)
        {
            switch (codec)
            {
                case Codec.H264: return "libx264";
                case Codec.H265: return "libx265";
                case Codec.VP9: return "libvpx-vp9";
                case Codec.ProRes: return "prores_ks";
                case Codec.AviRaw: return "rawvideo";
            }
            return "libx264";
        }

        public static string GetEncArgs (Codec codec)
        {
            string args = $"-c:v { GetEnc(codec)} ";

            switch (codec)
            {
                case Codec.H264: args += $"-crf {Config.GetInt("h264Crf")} -preset {Config.Get("ffEncPreset")}"; break;
                case Codec.H265: args += $"-crf {Config.GetInt("h265Crf")} -preset {Config.Get("ffEncPreset")}"; break;
                case Codec.VP9: args += $"-crf {Config.GetInt("vp9Crf")} -preset {Config.Get("ffEncPreset")}"; break;
                case Codec.ProRes: args += $"-profile:v {Config.GetInt("proResProfile")}"; break;
            }

            return args;
        }

        public static string GetAudioEnc(Codec codec)
        {
            switch (codec)
            {
                case Codec.VP9: return "libopus";
            }
            return "aac";
        }

        public static int GetAudioBitrate (string codec)
        {
            if (codec.Trim().ToLower() == "aac")
                return 128;
            if (codec.Trim().ToLower().Contains("opus"))
                return 112;

            return 160;
        }

        public static string GetExt(Interpolate.OutMode outMode, bool dot = true)
        {
            string ext = dot ? "." : "";

            switch (outMode)
            {
                case Interpolate.OutMode.VidMp4: ext += "mp4"; break;
                case Interpolate.OutMode.VidWebm: ext += "webm"; break;
                case Interpolate.OutMode.VidProRes: ext += "mov"; break;
                case Interpolate.OutMode.VidAviRaw: ext += "avi"; break;
                case Interpolate.OutMode.VidGif: ext += "gif"; break;
            }

            return ext;
        }
    }
}