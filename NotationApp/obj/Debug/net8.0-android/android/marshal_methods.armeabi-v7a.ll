; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [350 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [694 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 270
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 304
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39109920, ; 6: Newtonsoft.Json.dll => 0x254c520 => 218
	i32 39485524, ; 7: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 8: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 9: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 10: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 345
	i32 68219467, ; 11: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 12: Microsoft.Maui.Graphics.dll => 0x44bb714 => 211
	i32 82292897, ; 13: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 14: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 288
	i32 117431740, ; 15: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 16: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 288
	i32 122350210, ; 17: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 18: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 308
	i32 142721839, ; 19: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 20: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 151607338, ; 21: NAudio.Wasapi => 0x909582a => 216
	i32 159306688, ; 22: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 23: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 244
	i32 176265551, ; 24: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 25: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 290
	i32 184328833, ; 26: System.ValueTuple.dll => 0xafca281 => 151
	i32 195452805, ; 27: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 342
	i32 199333315, ; 28: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 343
	i32 205061960, ; 29: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 30: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 242
	i32 220171995, ; 31: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 32: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 264
	i32 230752869, ; 33: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 34: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 35: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 36: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 37: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 247
	i32 276479776, ; 38: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 39: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 266
	i32 280482487, ; 40: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 263
	i32 280992041, ; 41: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 314
	i32 291076382, ; 42: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 43: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 44: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 342
	i32 318968648, ; 45: Xamarin.AndroidX.Activity.dll => 0x13031348 => 233
	i32 321597661, ; 46: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 47: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 327
	i32 342366114, ; 48: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 265
	i32 347068432, ; 49: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 223
	i32 356389973, ; 50: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 326
	i32 360082299, ; 51: System.ServiceModel.Web => 0x15766b7b => 131
	i32 364956269, ; 52: Grpc.Net.Common => 0x15c0ca6d => 194
	i32 367780167, ; 53: System.IO.Pipes => 0x15ebe147 => 55
	i32 371306672, ; 54: Grpc.Core.Api.dll => 0x1621b0b0 => 192
	i32 374914964, ; 55: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 56: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 57: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 58: System.Memory.dll => 0x16fe439a => 62
	i32 391886110, ; 59: Grpc.Net.Client.dll => 0x175bb51e => 193
	i32 392610295, ; 60: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 393699800, ; 61: Firebase => 0x177761d8 => 177
	i32 395744057, ; 62: _Microsoft.Android.Resource.Designer => 0x17969339 => 346
	i32 403441872, ; 63: WindowsBase => 0x180c08d0 => 165
	i32 435591531, ; 64: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 338
	i32 441335492, ; 65: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 248
	i32 442565967, ; 66: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 67: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 261
	i32 451504562, ; 68: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 69: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 70: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 71: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 72: System.dll => 0x1bff388e => 164
	i32 476646585, ; 73: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 263
	i32 486930444, ; 74: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 276
	i32 498788369, ; 75: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 76: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 325
	i32 503918385, ; 77: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 319
	i32 513247710, ; 78: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 205
	i32 526420162, ; 79: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 80: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 308
	i32 530272170, ; 81: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 82: Microsoft.Extensions.Logging => 0x20216150 => 201
	i32 540030774, ; 83: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 84: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 85: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 548916678, ; 86: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 196
	i32 549171840, ; 87: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 88: Jsr305Binding => 0x213954e7 => 301
	i32 569601784, ; 89: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 299
	i32 577335427, ; 90: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 592146354, ; 91: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 333
	i32 597488923, ; 92: CommunityToolkit.Maui => 0x239cf51b => 173
	i32 601371474, ; 93: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 94: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 610194910, ; 95: System.Reactive.dll => 0x245ed5de => 228
	i32 613668793, ; 96: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 97: Xamarin.AndroidX.CustomView => 0x2568904f => 253
	i32 627931235, ; 98: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 331
	i32 639843206, ; 99: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 259
	i32 643868501, ; 100: System.Net => 0x2660a755 => 81
	i32 646990296, ; 101: Google.Cloud.Firestore.V1.dll => 0x269049d8 => 186
	i32 662205335, ; 102: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 103: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 295
	i32 666292255, ; 104: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 240
	i32 672442732, ; 105: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 106: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 107: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 313
	i32 690569205, ; 108: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 109: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 310
	i32 693804605, ; 110: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 111: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 112: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 305
	i32 700358131, ; 113: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 114: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 328
	i32 709557578, ; 115: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 316
	i32 720511267, ; 116: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 309
	i32 722857257, ; 117: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 118: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 748832960, ; 119: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 221
	i32 752232764, ; 120: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 121: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 230
	i32 759454413, ; 122: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 123: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 124: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 125: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 337
	i32 789151979, ; 126: Microsoft.Extensions.Options => 0x2f0980eb => 204
	i32 790371945, ; 127: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 254
	i32 804715423, ; 128: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 129: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 268
	i32 823281589, ; 130: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 131: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 132: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 133: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 134: Xamarin.AndroidX.Print => 0x3246f6cd => 281
	i32 873119928, ; 135: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 136: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 137: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 138: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 139: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 926902833, ; 140: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 340
	i32 928116545, ; 141: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 304
	i32 952186615, ; 142: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 143: Newtonsoft.Json => 0x38f24a24 => 218
	i32 956575887, ; 144: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 309
	i32 966729478, ; 145: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 302
	i32 967690846, ; 146: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 265
	i32 975236339, ; 147: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 148: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 149: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 150: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 151: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 152: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 153: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 154: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 285
	i32 1019214401, ; 155: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 156: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 200
	i32 1029334545, ; 157: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 315
	i32 1031528504, ; 158: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 303
	i32 1035644815, ; 159: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 238
	i32 1036536393, ; 160: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 161: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1049751285, ; 162: Google.Api.CommonProtos.dll => 0x3e91eef5 => 179
	i32 1052210849, ; 163: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 272
	i32 1067306892, ; 164: GoogleGson => 0x3f9dcf8c => 190
	i32 1082857460, ; 165: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 166: Xamarin.Kotlin.StdLib => 0x409e66d8 => 306
	i32 1098259244, ; 167: System => 0x41761b2c => 164
	i32 1118262833, ; 168: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 328
	i32 1121599056, ; 169: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 271
	i32 1127624469, ; 170: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 203
	i32 1145085672, ; 171: System.Linq.Async.dll => 0x44409ee8 => 226
	i32 1149092582, ; 172: Xamarin.AndroidX.Window => 0x447dc2e6 => 298
	i32 1168523401, ; 173: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 334
	i32 1170634674, ; 174: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 175: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 294
	i32 1178241025, ; 176: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 279
	i32 1203173028, ; 177: Grpc.Net.Client => 0x47b6f6a4 => 193
	i32 1203215381, ; 178: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 332
	i32 1204270330, ; 179: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 240
	i32 1208641965, ; 180: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 181: CommunityToolkit.Mvvm => 0x4868cc7b => 175
	i32 1219128291, ; 182: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1234928153, ; 183: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 330
	i32 1243150071, ; 184: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 299
	i32 1253011324, ; 185: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 186: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 314
	i32 1264511973, ; 187: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 289
	i32 1267360935, ; 188: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 293
	i32 1273260888, ; 189: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 245
	i32 1275534314, ; 190: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 310
	i32 1278448581, ; 191: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 237
	i32 1292207520, ; 192: SQLitePCLRaw.core.dll => 0x4d0585a0 => 222
	i32 1293217323, ; 193: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 256
	i32 1309188875, ; 194: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 195: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 298
	i32 1324164729, ; 196: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 197: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1347751866, ; 198: Plugin.Maui.Audio => 0x50550fba => 219
	i32 1364015309, ; 199: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 200: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 344
	i32 1376866003, ; 201: Xamarin.AndroidX.SavedState => 0x52114ed3 => 285
	i32 1379779777, ; 202: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 203: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 204: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 249
	i32 1408764838, ; 205: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 206: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 207: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 208: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 312
	i32 1434145427, ; 209: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 210: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 302
	i32 1437713837, ; 211: Grpc.Auth => 0x55b1c5ad => 191
	i32 1439761251, ; 212: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 213: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 214: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 215: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 216: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 217: es\Microsoft.Maui.Controls.resources => 0x57152abe => 318
	i32 1461234159, ; 218: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 219: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 220: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 221: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 239
	i32 1470490898, ; 222: Microsoft.Extensions.Primitives => 0x57a5e912 => 205
	i32 1479771757, ; 223: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 224: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 225: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1489956495, ; 226: NAudio.Core => 0x58ceee8f => 214
	i32 1490025113, ; 227: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 286
	i32 1493001747, ; 228: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 322
	i32 1495870163, ; 229: NAudio.Midi.dll => 0x59292ad3 => 215
	i32 1514721132, ; 230: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 317
	i32 1536373174, ; 231: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 232: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 233: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 234: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 235: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 337
	i32 1565862583, ; 236: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 237: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 238: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 239: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 240: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 255
	i32 1592978981, ; 241: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 242: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 303
	i32 1601112923, ; 243: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 244: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 245: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 246: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 275
	i32 1622358360, ; 247: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 248: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 297
	i32 1634654947, ; 249: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 174
	i32 1635184631, ; 250: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 259
	i32 1636350590, ; 251: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 252
	i32 1639515021, ; 252: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 253: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 254: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1643317837, ; 255: NotationApp => 0x61f30a4d => 0
	i32 1654915987, ; 256: NAudio => 0x62a40393 => 212
	i32 1657153582, ; 257: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 258: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 291
	i32 1658251792, ; 259: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 300
	i32 1670060433, ; 260: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 247
	i32 1675553242, ; 261: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 262: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 263: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 264: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 265: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 266: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 267: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 307
	i32 1701541528, ; 268: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1711441057, ; 269: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 223
	i32 1720223769, ; 270: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 268
	i32 1726116996, ; 271: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 272: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 273: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 243
	i32 1736233607, ; 274: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 335
	i32 1743415430, ; 275: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 313
	i32 1744735666, ; 276: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746316138, ; 277: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 278: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 279: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 280: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 281: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 282: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 290
	i32 1770582343, ; 283: Microsoft.Extensions.Logging.dll => 0x6988f147 => 201
	i32 1776026572, ; 284: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 285: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 286: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782161461, ; 287: Grpc.Core.Api => 0x6a39a035 => 192
	i32 1782862114, ; 288: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 329
	i32 1788241197, ; 289: Xamarin.AndroidX.Fragment => 0x6a96652d => 261
	i32 1793755602, ; 290: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 321
	i32 1796167890, ; 291: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 196
	i32 1808609942, ; 292: Xamarin.AndroidX.Loader => 0x6bcd3296 => 275
	i32 1813058853, ; 293: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 306
	i32 1813201214, ; 294: Xamarin.Google.Android.Material => 0x6c13413e => 300
	i32 1818569960, ; 295: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 280
	i32 1818787751, ; 296: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 297: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 298: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1828688058, ; 299: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 202
	i32 1842015223, ; 300: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 341
	i32 1847515442, ; 301: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 230
	i32 1853025655, ; 302: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 338
	i32 1858542181, ; 303: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 304: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 305: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 320
	i32 1879696579, ; 306: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 307: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 241
	i32 1888955245, ; 308: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 309: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 310: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900519031, ; 311: Grpc.Auth.dll => 0x71479e77 => 191
	i32 1900610850, ; 312: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 313: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1927897671, ; 314: System.CodeDom.dll => 0x72e96247 => 225
	i32 1939592360, ; 315: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 316: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 317: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 287
	i32 1968388702, ; 318: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 197
	i32 1983156543, ; 319: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 307
	i32 1985761444, ; 320: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 232
	i32 2003115576, ; 321: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 317
	i32 2011961780, ; 322: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 323: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 272
	i32 2025202353, ; 324: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 312
	i32 2031763787, ; 325: Xamarin.Android.Glide => 0x791a414b => 229
	i32 2045470958, ; 326: System.Private.Xml => 0x79eb68ee => 88
	i32 2055257422, ; 327: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 267
	i32 2060060697, ; 328: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 329: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 316
	i32 2070888862, ; 330: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 331: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 332: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 333: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 224
	i32 2127167465, ; 334: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 335: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 336: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 337: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 338: Microsoft.Maui => 0x80bd55ad => 209
	i32 2169148018, ; 339: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 324
	i32 2178612968, ; 340: System.CodeDom => 0x81dafee8 => 225
	i32 2181898931, ; 341: Microsoft.Extensions.Options.dll => 0x820d22b3 => 204
	i32 2192057212, ; 342: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 202
	i32 2193016926, ; 343: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2199201384, ; 344: NAudio.dll => 0x83152668 => 212
	i32 2201107256, ; 345: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 311
	i32 2201231467, ; 346: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 347: it\Microsoft.Maui.Controls.resources => 0x839595db => 326
	i32 2216717168, ; 348: Firebase.Auth.dll => 0x84206b70 => 176
	i32 2217644978, ; 349: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 294
	i32 2222056684, ; 350: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 351: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 276
	i32 2252106437, ; 352: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 353: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 354: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 355: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 198
	i32 2267999099, ; 356: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 231
	i32 2270573516, ; 357: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 320
	i32 2279755925, ; 358: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 283
	i32 2293034957, ; 359: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 360: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 361: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 362: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 330
	i32 2305521784, ; 363: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 364: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 235
	i32 2320631194, ; 365: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 366: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 367: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 368: System.Net.Primitives => 0x8c40e0db => 70
	i32 2361517922, ; 369: NotationApp.dll => 0x8cc1e762 => 0
	i32 2368005991, ; 370: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2371007202, ; 371: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 197
	i32 2378619854, ; 372: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 373: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2385305715, ; 374: NAudio.Wasapi.dll => 0x8e2ce073 => 216
	i32 2395872292, ; 375: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 325
	i32 2397347608, ; 376: Google.LongRunning.dll => 0x8ee49f18 => 188
	i32 2401565422, ; 377: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 378: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 258
	i32 2421380589, ; 379: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 380: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 245
	i32 2427813419, ; 381: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 322
	i32 2435356389, ; 382: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 383: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2441199521, ; 384: Google.Cloud.Firestore => 0x9181bfa1 => 185
	i32 2454642406, ; 385: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 386: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 387: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465273461, ; 388: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 221
	i32 2465532216, ; 389: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 248
	i32 2471841756, ; 390: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 391: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 392: Microsoft.Maui.Controls => 0x93dba8a1 => 207
	i32 2483903535, ; 393: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 394: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2486847491, ; 395: Google.Api.Gax => 0x943a4803 => 180
	i32 2490993605, ; 396: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 397: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 398: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 270
	i32 2522472828, ; 399: Xamarin.Android.Glide.dll => 0x9659e17c => 229
	i32 2538310050, ; 400: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 401: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 323
	i32 2562349572, ; 402: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 403: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 404: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 271
	i32 2581819634, ; 405: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 293
	i32 2585220780, ; 406: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 407: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 408: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2593496499, ; 409: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 332
	i32 2605712449, ; 410: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 311
	i32 2615233544, ; 411: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 262
	i32 2616218305, ; 412: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 203
	i32 2617129537, ; 413: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 414: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 415: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 252
	i32 2624644809, ; 416: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 257
	i32 2626831493, ; 417: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 327
	i32 2627185994, ; 418: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 419: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 420: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 266
	i32 2635732976, ; 421: Google.Cloud.Firestore.dll => 0x9d1a17f0 => 185
	i32 2663391936, ; 422: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 231
	i32 2663698177, ; 423: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 424: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 425: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 426: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 427: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 428: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 429: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 291
	i32 2715334215, ; 430: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 431: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 432: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 433: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 434: Xamarin.AndroidX.Activity => 0xa2e0939b => 233
	i32 2735172069, ; 435: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 436: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 239
	i32 2740948882, ; 437: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2744327253, ; 438: Google.Api.Gax.Grpc.dll => 0xa3931c55 => 181
	i32 2748088231, ; 439: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 440: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 333
	i32 2757554483, ; 441: Google.Api.Gax.Grpc => 0xa45cf133 => 181
	i32 2758225723, ; 442: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 208
	i32 2764765095, ; 443: Microsoft.Maui.dll => 0xa4caf7a7 => 209
	i32 2765824710, ; 444: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 445: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 305
	i32 2778768386, ; 446: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 296
	i32 2779977773, ; 447: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 284
	i32 2785988530, ; 448: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 339
	i32 2788224221, ; 449: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 262
	i32 2801831435, ; 450: Microsoft.Maui.Graphics => 0xa7008e0b => 211
	i32 2803228030, ; 451: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806116107, ; 452: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 318
	i32 2810250172, ; 453: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 249
	i32 2818335264, ; 454: System.Linq.Async => 0xa7fc6220 => 226
	i32 2819470561, ; 455: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 456: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 457: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 284
	i32 2824502124, ; 458: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2831556043, ; 459: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 331
	i32 2838993487, ; 460: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 273
	i32 2839679515, ; 461: Google.LongRunning => 0xa942121b => 188
	i32 2849599387, ; 462: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 463: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 296
	i32 2855708567, ; 464: Xamarin.AndroidX.Transition => 0xaa36a797 => 292
	i32 2861098320, ; 465: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 466: Microsoft.Maui.Essentials => 0xaa8a4878 => 210
	i32 2868488919, ; 467: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 174
	i32 2870099610, ; 468: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 234
	i32 2875164099, ; 469: Jsr305Binding.dll => 0xab5f85c3 => 301
	i32 2875220617, ; 470: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 471: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 260
	i32 2887636118, ; 472: System.Net.dll => 0xac1dd496 => 81
	i32 2893550578, ; 473: Google.Apis.Core => 0xac7813f2 => 184
	i32 2898407901, ; 474: System.Management => 0xacc231dd => 227
	i32 2899753641, ; 475: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 476: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 477: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 478: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 479: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2912646636, ; 480: Google.Api.CommonProtos => 0xad9b75ec => 179
	i32 2916838712, ; 481: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 297
	i32 2919462931, ; 482: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 483: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 236
	i32 2936416060, ; 484: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 485: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 486: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 487: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 488: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 489: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 490: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 256
	i32 2987532451, ; 491: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 287
	i32 2990604888, ; 492: Google.Apis => 0xb2410258 => 182
	i32 2991755712, ; 493: NAudio.Asio.dll => 0xb25291c0 => 213
	i32 2996846495, ; 494: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 269
	i32 3016983068, ; 495: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 289
	i32 3023353419, ; 496: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 497: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 264
	i32 3038032645, ; 498: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 346
	i32 3056245963, ; 499: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 286
	i32 3057625584, ; 500: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 277
	i32 3059408633, ; 501: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 502: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3061151445, ; 503: NAudio.WinMM => 0xb67576d5 => 217
	i32 3075834255, ; 504: System.Threading.Tasks => 0xb755818f => 144
	i32 3077302341, ; 505: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 324
	i32 3090735792, ; 506: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 507: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 508: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3106263381, ; 509: Grpc.Net.Common.dll => 0xb925d155 => 194
	i32 3111772706, ; 510: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 511: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 512: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 513: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 514: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 515: GoogleGson.dll => 0xbba64c02 => 190
	i32 3159123045, ; 516: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 517: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3178803400, ; 518: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 278
	i32 3192346100, ; 519: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 520: System.Web => 0xbe592c0c => 153
	i32 3203277885, ; 521: Google.Api.Gax.dll => 0xbeee243d => 180
	i32 3204380047, ; 522: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 523: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 524: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 255
	i32 3220365878, ; 525: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 526: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 527: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 528: Xamarin.AndroidX.CardView => 0xc235e84d => 243
	i32 3265493905, ; 529: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 530: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 531: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 532: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 533: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3286872994, ; 534: SQLite-net.dll => 0xc3e9b3a2 => 220
	i32 3290767353, ; 535: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 536: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 537: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 538: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 319
	i32 3316684772, ; 539: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 540: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 253
	i32 3317144872, ; 541: System.Data => 0xc5b79d28 => 24
	i32 3322403133, ; 542: Firebase.dll => 0xc607d93d => 177
	i32 3340431453, ; 543: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 241
	i32 3345895724, ; 544: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 282
	i32 3346324047, ; 545: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 279
	i32 3357674450, ; 546: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 336
	i32 3358260929, ; 547: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 548: SQLitePCLRaw.core => 0xc849ca45 => 222
	i32 3362336904, ; 549: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 234
	i32 3362522851, ; 550: Xamarin.AndroidX.Core => 0xc86c06e3 => 250
	i32 3366347497, ; 551: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 552: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 283
	i32 3381016424, ; 553: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 315
	i32 3395150330, ; 554: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 555: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 556: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 254
	i32 3428513518, ; 557: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 199
	i32 3429136800, ; 558: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 559: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 560: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 257
	i32 3445260447, ; 561: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 562: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 206
	i32 3453592554, ; 563: Google.Apis.Core.dll => 0xcdd9a3ea => 184
	i32 3463511458, ; 564: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 323
	i32 3471940407, ; 565: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 566: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 567: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 336
	i32 3484440000, ; 568: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 335
	i32 3485117614, ; 569: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 570: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 571: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 246
	i32 3499097210, ; 572: Google.Protobuf.dll => 0xd08ffc7a => 189
	i32 3508241679, ; 573: NAudio.Asio => 0xd11b850f => 213
	i32 3509114376, ; 574: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 575: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 576: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 577: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 578: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 579: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 580: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 343
	i32 3596207933, ; 581: LiteDB.dll => 0xd659c73d => 195
	i32 3597029428, ; 582: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 232
	i32 3598063517, ; 583: Google.Cloud.Firestore.V1 => 0xd676179d => 186
	i32 3598340787, ; 584: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 585: System.Linq.dll => 0xd715a361 => 61
	i32 3612435020, ; 586: System.Management.dll => 0xd751624c => 227
	i32 3624195450, ; 587: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 588: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 281
	i32 3629588173, ; 589: LiteDB => 0xd8571ecd => 195
	i32 3633644679, ; 590: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 236
	i32 3638274909, ; 591: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 592: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 267
	i32 3643446276, ; 593: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 340
	i32 3643854240, ; 594: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 278
	i32 3645089577, ; 595: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3645630983, ; 596: Google.Protobuf => 0xd94bea07 => 189
	i32 3655481159, ; 597: Firebase.Storage => 0xd9e23747 => 178
	i32 3657292374, ; 598: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 198
	i32 3660523487, ; 599: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3660726404, ; 600: Plugin.Maui.Audio.dll => 0xda324084 => 219
	i32 3672681054, ; 601: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 602: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 242
	i32 3684561358, ; 603: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 246
	i32 3697841164, ; 604: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 345
	i32 3700866549, ; 605: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 606: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 251
	i32 3716563718, ; 607: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 608: Xamarin.AndroidX.Annotation => 0xdda814c6 => 235
	i32 3724971120, ; 609: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 277
	i32 3731644420, ; 610: System.Reactive => 0xde6c6004 => 228
	i32 3732100267, ; 611: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 612: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 613: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 614: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3754567612, ; 615: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 224
	i32 3757995660, ; 616: Google.Cloud.Location.dll => 0xdffe768c => 187
	i32 3786282454, ; 617: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 244
	i32 3792276235, ; 618: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3793997468, ; 619: Google.Apis.Auth.dll => 0xe223ce9c => 183
	i32 3800979733, ; 620: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 206
	i32 3802395368, ; 621: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3817368567, ; 622: CommunityToolkit.Maui.dll => 0xe3886bf7 => 173
	i32 3819260425, ; 623: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 624: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 625: System.Numerics.dll => 0xe4436460 => 83
	i32 3831870545, ; 626: NAudio.WinMM.dll => 0xe465b451 => 217
	i32 3833826689, ; 627: NAudio.Core.dll => 0xe4838d81 => 214
	i32 3841636137, ; 628: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 200
	i32 3844307129, ; 629: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 630: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 631: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 632: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 633: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 634: SQLite-net => 0xe70c9739 => 220
	i32 3885497537, ; 635: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 636: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 292
	i32 3887389151, ; 637: NAudio.Midi => 0xe7b4d9df => 215
	i32 3888767677, ; 638: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 282
	i32 3889960447, ; 639: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 344
	i32 3896106733, ; 640: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 641: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 250
	i32 3901907137, ; 642: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 643: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 644: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 295
	i32 3928044579, ; 645: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3929187773, ; 646: Firebase.Storage.dll => 0xea32a5bd => 178
	i32 3930554604, ; 647: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 648: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 280
	i32 3945713374, ; 649: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 650: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 651: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 238
	i32 3959773229, ; 652: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 269
	i32 3980434154, ; 653: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 339
	i32 3987592930, ; 654: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 321
	i32 4003436829, ; 655: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 656: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 237
	i32 4024013275, ; 657: Firebase.Auth => 0xefd991db => 176
	i32 4025784931, ; 658: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 659: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 208
	i32 4054681211, ; 660: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4056144661, ; 661: Google.Cloud.Location => 0xf1c3db15 => 187
	i32 4059682726, ; 662: Google.Apis.dll => 0xf1f9d7a6 => 182
	i32 4068434129, ; 663: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 664: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4082882467, ; 665: Google.Apis.Auth => 0xf35bd7a3 => 183
	i32 4094352644, ; 666: Microsoft.Maui.Essentials.dll => 0xf40add04 => 210
	i32 4099507663, ; 667: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 668: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 669: Xamarin.AndroidX.Emoji2 => 0xf479582c => 258
	i32 4102112229, ; 670: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 334
	i32 4125707920, ; 671: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 329
	i32 4126470640, ; 672: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 199
	i32 4127667938, ; 673: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 674: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 675: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 676: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 341
	i32 4151237749, ; 677: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 678: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 679: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 680: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 681: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 682: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 274
	i32 4185676441, ; 683: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 684: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 685: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 686: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 251
	i32 4258378803, ; 687: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 273
	i32 4260525087, ; 688: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 689: Microsoft.Maui.Controls.dll => 0xfea12dee => 207
	i32 4274623895, ; 690: CommunityToolkit.Mvvm.dll => 0xfec99597 => 175
	i32 4274976490, ; 691: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 692: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 274
	i32 4294763496 ; 693: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 260
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [694 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 270, ; 3
	i32 304, ; 4
	i32 48, ; 5
	i32 218, ; 6
	i32 80, ; 7
	i32 145, ; 8
	i32 30, ; 9
	i32 345, ; 10
	i32 124, ; 11
	i32 211, ; 12
	i32 102, ; 13
	i32 288, ; 14
	i32 107, ; 15
	i32 288, ; 16
	i32 139, ; 17
	i32 308, ; 18
	i32 77, ; 19
	i32 124, ; 20
	i32 216, ; 21
	i32 13, ; 22
	i32 244, ; 23
	i32 132, ; 24
	i32 290, ; 25
	i32 151, ; 26
	i32 342, ; 27
	i32 343, ; 28
	i32 18, ; 29
	i32 242, ; 30
	i32 26, ; 31
	i32 264, ; 32
	i32 1, ; 33
	i32 59, ; 34
	i32 42, ; 35
	i32 91, ; 36
	i32 247, ; 37
	i32 147, ; 38
	i32 266, ; 39
	i32 263, ; 40
	i32 314, ; 41
	i32 54, ; 42
	i32 69, ; 43
	i32 342, ; 44
	i32 233, ; 45
	i32 83, ; 46
	i32 327, ; 47
	i32 265, ; 48
	i32 223, ; 49
	i32 326, ; 50
	i32 131, ; 51
	i32 194, ; 52
	i32 55, ; 53
	i32 192, ; 54
	i32 149, ; 55
	i32 74, ; 56
	i32 145, ; 57
	i32 62, ; 58
	i32 193, ; 59
	i32 146, ; 60
	i32 177, ; 61
	i32 346, ; 62
	i32 165, ; 63
	i32 338, ; 64
	i32 248, ; 65
	i32 12, ; 66
	i32 261, ; 67
	i32 125, ; 68
	i32 152, ; 69
	i32 113, ; 70
	i32 166, ; 71
	i32 164, ; 72
	i32 263, ; 73
	i32 276, ; 74
	i32 84, ; 75
	i32 325, ; 76
	i32 319, ; 77
	i32 205, ; 78
	i32 150, ; 79
	i32 308, ; 80
	i32 60, ; 81
	i32 201, ; 82
	i32 51, ; 83
	i32 103, ; 84
	i32 114, ; 85
	i32 196, ; 86
	i32 40, ; 87
	i32 301, ; 88
	i32 299, ; 89
	i32 120, ; 90
	i32 333, ; 91
	i32 173, ; 92
	i32 52, ; 93
	i32 44, ; 94
	i32 228, ; 95
	i32 119, ; 96
	i32 253, ; 97
	i32 331, ; 98
	i32 259, ; 99
	i32 81, ; 100
	i32 186, ; 101
	i32 136, ; 102
	i32 295, ; 103
	i32 240, ; 104
	i32 8, ; 105
	i32 73, ; 106
	i32 313, ; 107
	i32 155, ; 108
	i32 310, ; 109
	i32 154, ; 110
	i32 92, ; 111
	i32 305, ; 112
	i32 45, ; 113
	i32 328, ; 114
	i32 316, ; 115
	i32 309, ; 116
	i32 109, ; 117
	i32 129, ; 118
	i32 221, ; 119
	i32 25, ; 120
	i32 230, ; 121
	i32 72, ; 122
	i32 55, ; 123
	i32 46, ; 124
	i32 337, ; 125
	i32 204, ; 126
	i32 254, ; 127
	i32 22, ; 128
	i32 268, ; 129
	i32 86, ; 130
	i32 43, ; 131
	i32 160, ; 132
	i32 71, ; 133
	i32 281, ; 134
	i32 3, ; 135
	i32 42, ; 136
	i32 63, ; 137
	i32 16, ; 138
	i32 53, ; 139
	i32 340, ; 140
	i32 304, ; 141
	i32 105, ; 142
	i32 218, ; 143
	i32 309, ; 144
	i32 302, ; 145
	i32 265, ; 146
	i32 34, ; 147
	i32 158, ; 148
	i32 85, ; 149
	i32 32, ; 150
	i32 12, ; 151
	i32 51, ; 152
	i32 56, ; 153
	i32 285, ; 154
	i32 36, ; 155
	i32 200, ; 156
	i32 315, ; 157
	i32 303, ; 158
	i32 238, ; 159
	i32 35, ; 160
	i32 58, ; 161
	i32 179, ; 162
	i32 272, ; 163
	i32 190, ; 164
	i32 17, ; 165
	i32 306, ; 166
	i32 164, ; 167
	i32 328, ; 168
	i32 271, ; 169
	i32 203, ; 170
	i32 226, ; 171
	i32 298, ; 172
	i32 334, ; 173
	i32 153, ; 174
	i32 294, ; 175
	i32 279, ; 176
	i32 193, ; 177
	i32 332, ; 178
	i32 240, ; 179
	i32 29, ; 180
	i32 175, ; 181
	i32 52, ; 182
	i32 330, ; 183
	i32 299, ; 184
	i32 5, ; 185
	i32 314, ; 186
	i32 289, ; 187
	i32 293, ; 188
	i32 245, ; 189
	i32 310, ; 190
	i32 237, ; 191
	i32 222, ; 192
	i32 256, ; 193
	i32 85, ; 194
	i32 298, ; 195
	i32 61, ; 196
	i32 112, ; 197
	i32 219, ; 198
	i32 57, ; 199
	i32 344, ; 200
	i32 285, ; 201
	i32 99, ; 202
	i32 19, ; 203
	i32 249, ; 204
	i32 111, ; 205
	i32 101, ; 206
	i32 102, ; 207
	i32 312, ; 208
	i32 104, ; 209
	i32 302, ; 210
	i32 191, ; 211
	i32 71, ; 212
	i32 38, ; 213
	i32 32, ; 214
	i32 103, ; 215
	i32 73, ; 216
	i32 318, ; 217
	i32 9, ; 218
	i32 123, ; 219
	i32 46, ; 220
	i32 239, ; 221
	i32 205, ; 222
	i32 9, ; 223
	i32 43, ; 224
	i32 4, ; 225
	i32 214, ; 226
	i32 286, ; 227
	i32 322, ; 228
	i32 215, ; 229
	i32 317, ; 230
	i32 31, ; 231
	i32 138, ; 232
	i32 92, ; 233
	i32 93, ; 234
	i32 337, ; 235
	i32 49, ; 236
	i32 141, ; 237
	i32 112, ; 238
	i32 140, ; 239
	i32 255, ; 240
	i32 115, ; 241
	i32 303, ; 242
	i32 157, ; 243
	i32 76, ; 244
	i32 79, ; 245
	i32 275, ; 246
	i32 37, ; 247
	i32 297, ; 248
	i32 174, ; 249
	i32 259, ; 250
	i32 252, ; 251
	i32 64, ; 252
	i32 138, ; 253
	i32 15, ; 254
	i32 0, ; 255
	i32 212, ; 256
	i32 116, ; 257
	i32 291, ; 258
	i32 300, ; 259
	i32 247, ; 260
	i32 48, ; 261
	i32 70, ; 262
	i32 80, ; 263
	i32 126, ; 264
	i32 94, ; 265
	i32 121, ; 266
	i32 307, ; 267
	i32 26, ; 268
	i32 223, ; 269
	i32 268, ; 270
	i32 97, ; 271
	i32 28, ; 272
	i32 243, ; 273
	i32 335, ; 274
	i32 313, ; 275
	i32 149, ; 276
	i32 169, ; 277
	i32 4, ; 278
	i32 98, ; 279
	i32 33, ; 280
	i32 93, ; 281
	i32 290, ; 282
	i32 201, ; 283
	i32 21, ; 284
	i32 41, ; 285
	i32 170, ; 286
	i32 192, ; 287
	i32 329, ; 288
	i32 261, ; 289
	i32 321, ; 290
	i32 196, ; 291
	i32 275, ; 292
	i32 306, ; 293
	i32 300, ; 294
	i32 280, ; 295
	i32 2, ; 296
	i32 134, ; 297
	i32 111, ; 298
	i32 202, ; 299
	i32 341, ; 300
	i32 230, ; 301
	i32 338, ; 302
	i32 58, ; 303
	i32 95, ; 304
	i32 320, ; 305
	i32 39, ; 306
	i32 241, ; 307
	i32 25, ; 308
	i32 94, ; 309
	i32 89, ; 310
	i32 191, ; 311
	i32 99, ; 312
	i32 10, ; 313
	i32 225, ; 314
	i32 87, ; 315
	i32 100, ; 316
	i32 287, ; 317
	i32 197, ; 318
	i32 307, ; 319
	i32 232, ; 320
	i32 317, ; 321
	i32 7, ; 322
	i32 272, ; 323
	i32 312, ; 324
	i32 229, ; 325
	i32 88, ; 326
	i32 267, ; 327
	i32 154, ; 328
	i32 316, ; 329
	i32 33, ; 330
	i32 116, ; 331
	i32 82, ; 332
	i32 224, ; 333
	i32 20, ; 334
	i32 11, ; 335
	i32 162, ; 336
	i32 3, ; 337
	i32 209, ; 338
	i32 324, ; 339
	i32 225, ; 340
	i32 204, ; 341
	i32 202, ; 342
	i32 84, ; 343
	i32 212, ; 344
	i32 311, ; 345
	i32 64, ; 346
	i32 326, ; 347
	i32 176, ; 348
	i32 294, ; 349
	i32 143, ; 350
	i32 276, ; 351
	i32 157, ; 352
	i32 41, ; 353
	i32 117, ; 354
	i32 198, ; 355
	i32 231, ; 356
	i32 320, ; 357
	i32 283, ; 358
	i32 131, ; 359
	i32 75, ; 360
	i32 66, ; 361
	i32 330, ; 362
	i32 172, ; 363
	i32 235, ; 364
	i32 143, ; 365
	i32 106, ; 366
	i32 151, ; 367
	i32 70, ; 368
	i32 0, ; 369
	i32 156, ; 370
	i32 197, ; 371
	i32 121, ; 372
	i32 127, ; 373
	i32 216, ; 374
	i32 325, ; 375
	i32 188, ; 376
	i32 152, ; 377
	i32 258, ; 378
	i32 141, ; 379
	i32 245, ; 380
	i32 322, ; 381
	i32 20, ; 382
	i32 14, ; 383
	i32 185, ; 384
	i32 135, ; 385
	i32 75, ; 386
	i32 59, ; 387
	i32 221, ; 388
	i32 248, ; 389
	i32 167, ; 390
	i32 168, ; 391
	i32 207, ; 392
	i32 15, ; 393
	i32 74, ; 394
	i32 180, ; 395
	i32 6, ; 396
	i32 23, ; 397
	i32 270, ; 398
	i32 229, ; 399
	i32 91, ; 400
	i32 323, ; 401
	i32 1, ; 402
	i32 136, ; 403
	i32 271, ; 404
	i32 293, ; 405
	i32 134, ; 406
	i32 69, ; 407
	i32 146, ; 408
	i32 332, ; 409
	i32 311, ; 410
	i32 262, ; 411
	i32 203, ; 412
	i32 88, ; 413
	i32 96, ; 414
	i32 252, ; 415
	i32 257, ; 416
	i32 327, ; 417
	i32 31, ; 418
	i32 45, ; 419
	i32 266, ; 420
	i32 185, ; 421
	i32 231, ; 422
	i32 109, ; 423
	i32 158, ; 424
	i32 35, ; 425
	i32 22, ; 426
	i32 114, ; 427
	i32 57, ; 428
	i32 291, ; 429
	i32 144, ; 430
	i32 118, ; 431
	i32 120, ; 432
	i32 110, ; 433
	i32 233, ; 434
	i32 139, ; 435
	i32 239, ; 436
	i32 54, ; 437
	i32 181, ; 438
	i32 105, ; 439
	i32 333, ; 440
	i32 181, ; 441
	i32 208, ; 442
	i32 209, ; 443
	i32 133, ; 444
	i32 305, ; 445
	i32 296, ; 446
	i32 284, ; 447
	i32 339, ; 448
	i32 262, ; 449
	i32 211, ; 450
	i32 159, ; 451
	i32 318, ; 452
	i32 249, ; 453
	i32 226, ; 454
	i32 163, ; 455
	i32 132, ; 456
	i32 284, ; 457
	i32 161, ; 458
	i32 331, ; 459
	i32 273, ; 460
	i32 188, ; 461
	i32 140, ; 462
	i32 296, ; 463
	i32 292, ; 464
	i32 169, ; 465
	i32 210, ; 466
	i32 174, ; 467
	i32 234, ; 468
	i32 301, ; 469
	i32 40, ; 470
	i32 260, ; 471
	i32 81, ; 472
	i32 184, ; 473
	i32 227, ; 474
	i32 56, ; 475
	i32 37, ; 476
	i32 97, ; 477
	i32 166, ; 478
	i32 172, ; 479
	i32 179, ; 480
	i32 297, ; 481
	i32 82, ; 482
	i32 236, ; 483
	i32 98, ; 484
	i32 30, ; 485
	i32 159, ; 486
	i32 18, ; 487
	i32 127, ; 488
	i32 119, ; 489
	i32 256, ; 490
	i32 287, ; 491
	i32 182, ; 492
	i32 213, ; 493
	i32 269, ; 494
	i32 289, ; 495
	i32 165, ; 496
	i32 264, ; 497
	i32 346, ; 498
	i32 286, ; 499
	i32 277, ; 500
	i32 170, ; 501
	i32 16, ; 502
	i32 217, ; 503
	i32 144, ; 504
	i32 324, ; 505
	i32 125, ; 506
	i32 118, ; 507
	i32 38, ; 508
	i32 194, ; 509
	i32 115, ; 510
	i32 47, ; 511
	i32 142, ; 512
	i32 117, ; 513
	i32 34, ; 514
	i32 190, ; 515
	i32 95, ; 516
	i32 53, ; 517
	i32 278, ; 518
	i32 129, ; 519
	i32 153, ; 520
	i32 180, ; 521
	i32 24, ; 522
	i32 161, ; 523
	i32 255, ; 524
	i32 148, ; 525
	i32 104, ; 526
	i32 89, ; 527
	i32 243, ; 528
	i32 60, ; 529
	i32 142, ; 530
	i32 100, ; 531
	i32 5, ; 532
	i32 13, ; 533
	i32 220, ; 534
	i32 122, ; 535
	i32 135, ; 536
	i32 28, ; 537
	i32 319, ; 538
	i32 72, ; 539
	i32 253, ; 540
	i32 24, ; 541
	i32 177, ; 542
	i32 241, ; 543
	i32 282, ; 544
	i32 279, ; 545
	i32 336, ; 546
	i32 137, ; 547
	i32 222, ; 548
	i32 234, ; 549
	i32 250, ; 550
	i32 168, ; 551
	i32 283, ; 552
	i32 315, ; 553
	i32 101, ; 554
	i32 123, ; 555
	i32 254, ; 556
	i32 199, ; 557
	i32 163, ; 558
	i32 167, ; 559
	i32 257, ; 560
	i32 39, ; 561
	i32 206, ; 562
	i32 184, ; 563
	i32 323, ; 564
	i32 17, ; 565
	i32 171, ; 566
	i32 336, ; 567
	i32 335, ; 568
	i32 137, ; 569
	i32 150, ; 570
	i32 246, ; 571
	i32 189, ; 572
	i32 213, ; 573
	i32 155, ; 574
	i32 130, ; 575
	i32 19, ; 576
	i32 65, ; 577
	i32 147, ; 578
	i32 47, ; 579
	i32 343, ; 580
	i32 195, ; 581
	i32 232, ; 582
	i32 186, ; 583
	i32 79, ; 584
	i32 61, ; 585
	i32 227, ; 586
	i32 106, ; 587
	i32 281, ; 588
	i32 195, ; 589
	i32 236, ; 590
	i32 49, ; 591
	i32 267, ; 592
	i32 340, ; 593
	i32 278, ; 594
	i32 14, ; 595
	i32 189, ; 596
	i32 178, ; 597
	i32 198, ; 598
	i32 68, ; 599
	i32 219, ; 600
	i32 171, ; 601
	i32 242, ; 602
	i32 246, ; 603
	i32 345, ; 604
	i32 78, ; 605
	i32 251, ; 606
	i32 108, ; 607
	i32 235, ; 608
	i32 277, ; 609
	i32 228, ; 610
	i32 67, ; 611
	i32 63, ; 612
	i32 27, ; 613
	i32 160, ; 614
	i32 224, ; 615
	i32 187, ; 616
	i32 244, ; 617
	i32 10, ; 618
	i32 183, ; 619
	i32 206, ; 620
	i32 11, ; 621
	i32 173, ; 622
	i32 78, ; 623
	i32 126, ; 624
	i32 83, ; 625
	i32 217, ; 626
	i32 214, ; 627
	i32 200, ; 628
	i32 66, ; 629
	i32 107, ; 630
	i32 65, ; 631
	i32 128, ; 632
	i32 122, ; 633
	i32 220, ; 634
	i32 77, ; 635
	i32 292, ; 636
	i32 215, ; 637
	i32 282, ; 638
	i32 344, ; 639
	i32 8, ; 640
	i32 250, ; 641
	i32 2, ; 642
	i32 44, ; 643
	i32 295, ; 644
	i32 156, ; 645
	i32 178, ; 646
	i32 128, ; 647
	i32 280, ; 648
	i32 23, ; 649
	i32 133, ; 650
	i32 238, ; 651
	i32 269, ; 652
	i32 339, ; 653
	i32 321, ; 654
	i32 29, ; 655
	i32 237, ; 656
	i32 176, ; 657
	i32 62, ; 658
	i32 208, ; 659
	i32 90, ; 660
	i32 187, ; 661
	i32 182, ; 662
	i32 87, ; 663
	i32 148, ; 664
	i32 183, ; 665
	i32 210, ; 666
	i32 36, ; 667
	i32 86, ; 668
	i32 258, ; 669
	i32 334, ; 670
	i32 329, ; 671
	i32 199, ; 672
	i32 50, ; 673
	i32 6, ; 674
	i32 90, ; 675
	i32 341, ; 676
	i32 21, ; 677
	i32 162, ; 678
	i32 96, ; 679
	i32 50, ; 680
	i32 113, ; 681
	i32 274, ; 682
	i32 130, ; 683
	i32 76, ; 684
	i32 27, ; 685
	i32 251, ; 686
	i32 273, ; 687
	i32 7, ; 688
	i32 207, ; 689
	i32 175, ; 690
	i32 110, ; 691
	i32 274, ; 692
	i32 260 ; 693
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ 45bb7f365f1587a7786d5bbbf0daec075d5aabea"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
