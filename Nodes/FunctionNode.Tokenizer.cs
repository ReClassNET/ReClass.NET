using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class FunctionNode : BaseNode
	{
		private IntPtr address = IntPtr.Zero;
		private readonly List<string> instructions = new List<string>();
		private readonly List<Tuple<string, string, string>> instructions2 = new List<Tuple<string, string, string>>();

		public string Signature { get; set; } = "void function()";

		public ClassNode BelongsToClass { get; set; }

		private int memorySize = IntPtr.Size;
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => memorySize;

		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			DisassembleRemoteCode(memory, spot.Address);

			return string.Join("\n", instructions);
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Function") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			var ptr = view.Address.Add(Offset);

			DisassembleRemoteCode(view.Memory, ptr);

			if (levelsOpen[view.Level])
			{
				y += view.Font.Height;
				x = AddText(view, tx, y, view.Settings.TypeColor, HotSpot.NoneId, "Signature:") + view.Font.Width;
				x = AddText(view, x, y, view.Settings.ValueColor, 0, Signature);

				y += view.Font.Height;
				x = AddText(view, tx, y, view.Settings.TextColor, HotSpot.NoneId, "Belongs to: ");
				x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, BelongsToClass == null ? "<None>" : $"<{BelongsToClass.Name}>");
				x = AddIcon(view, x, y, Icons.Change, 1, HotSpotType.ChangeType);

				var minWidth = 26 * view.Font.Width;

				var addressColor = Color.FromArgb(128, 128, 128);

				y += 4;
				foreach (var line in instructions2)
				{
					y += view.Font.Height;

					//AddText(view, tx, y, view.Settings.NameColor, HotSpot.ReadOnlyId, line);
					x = AddText(view, tx, y, view.Settings.AddressColor, HotSpot.ReadOnlyId, line.Item1) + 15;
					x = Math.Max(AddText(view, x, y, view.Settings.HexColor, HotSpot.ReadOnlyId, line.Item2), x + minWidth) + 6;

					/*foreach (var token in new AssemblerTokenizer().Read(line.Item3))
					{
						var color = Color.Black;
						if (token.TokenType == TokenType.Keyword)
						{
							if (AssemblerTokenizer.IsKeyword(token.Value))
							{
								color = Color.Navy;
							}
							else if (AssemblerTokenizer.IsRegister(token.Value))
							{
								color = Color.Green;
							}
						}
						else if (token.TokenType == TokenType.Bracket)
						{
							color = Color.Purple;
						}
						else if (token.TokenType == TokenType.Operation)
						{
							color = Color.Red;
						}

						x = AddText(view, x, y, color, HotSpot.NoneId, token.Value) + 5;
					}*/
					AddText(view, x, y, view.Settings.ValueColor, HotSpot.ReadOnlyId, line.Item3);

				}
				y += 4;
			}

			return y + view.Font.Height;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				h += instructions.Count() * view.Font.Height;
			}
			return h;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0) // Signature
			{
				Signature = spot.Text;
			}
		}

		private void DisassembleRemoteCode(MemoryBuffer memory, IntPtr address)
		{
			Contract.Requires(memory != null);

			if (this.address != address)
			{
				instructions.Clear();

				this.address = address;

				if (!address.IsNull() && memory.Process.IsValid)
				{
					memorySize = 0;

					var disassembler = new Disassembler(memory.Process.CoreFunctions);
					foreach (var instruction in disassembler.RemoteDisassembleFunction(memory.Process, address, 8192))
					{
						memorySize += instruction.Length;

						instructions.Add($"{instruction.Address.ToString(Constants.StringHexFormat)} {instruction.Instruction}");
						instructions2.Add(Tuple.Create(instruction.Address.ToString(Constants.StringHexFormat), Utils.BytesToString(instruction.Data, instruction.Length), instruction.Instruction));
					}

					ParentNode?.ChildHasChanged(this);
				}
			}
		}
	}

	enum TokenType
	{
		Number,
		Keyword,
		Operation,
		Bracket,
		Register,
		Text
	}

	class Token
	{
		/// <summary>The type of the token.</summary>
		public TokenType TokenType { get; }

		/// <summary>The value of the token.</summary>
		public string Value { get; }

		public Token(TokenType type, string value)
		{
			Contract.Requires(value != null);

			TokenType = type;
			Value = value;
		}

		public override string ToString()
		{
			return $"{TokenType} {Value}";
		}
	}

	class AssemblerTokenizer
	{
		public List<Token> Read(string instruction)
		{
			Contract.Requires(instruction != null);

			var tokens = new List<Token>();

			var characters = instruction.ToCharArray();
			for (var i = 0; i < characters.Length; ++i)
			{
				if (IsPartOfKeyword(characters[i], true))
				{
					var buffer = characters[i].ToString();
					while (++i < characters.Length && IsPartOfKeyword(characters[i], false))
					{
						buffer += characters[i];
					}

					if (IsRegister(buffer))
					{
						tokens.Add(new Token(TokenType.Register, buffer));
					}
					else if (IsKeyword(buffer))
					{
						tokens.Add(new Token(TokenType.Keyword, buffer));
					}
					else
					{
						tokens.Add(new Token(TokenType.Text, buffer));
					}

					if (i == characters.Length)
					{
						continue;
					}
				}

				if (IsPartOfNumber(characters[i], true))
				{
					var buffer = characters[i].ToString();
					while (++i < characters.Length && IsPartOfNumber(characters[i], false))
					{
						buffer += characters[i];
					}

					tokens.Add(new Token(TokenType.Number, buffer));

					if (i == characters.Length)
					{
						continue;
					}
				}

				switch (characters[i])
				{
					case ' ':
						continue;
					case ',':
					case ':':
						tokens.Add(new Token(TokenType.Text, characters[i].ToString()));
						break;
					case '+':
					case '-':
					case '*':
					case '/':
						tokens.Add(new Token(TokenType.Operation, characters[i].ToString()));
						break;
					case '[':
					case ']':
						tokens.Add(new Token(TokenType.Bracket, characters[i].ToString()));
						break;
					default:
						throw new Exception($"Invalid token '{characters[i]}' detected at position {i}.");
				}
			}

			return tokens;
		}

		private bool IsPartOfKeyword(char character, bool isFirstCharacter)
		{
			return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z') || (!isFirstCharacter && (character >= '0' && character <= '9'));
		}

		private bool IsPartOfNumber(char character, bool isFirstCharacter)
		{
			return (character >= '0' && character <= '9') || (character >= 'a' && character <= 'f') || (character >= 'A' && character <= 'F') || (!isFirstCharacter && (character == 'x' || character == 'X'));
		}

		private static HashSet<string> Registers = new HashSet<string>
		{
			"rax", "eax", "ax", "al", "ah",
			"rbx", "ebx", "bx", "bl", "bh",
			"rcx", "ecx", "cx", "cl", "ch",
			"rdx", "edx", "dx", "dl", "dh",
			"rsi", "esi", "si", "sil",
			"rdi", "edi", "di", "dil",
			"rbp", "ebp", "bp", "bpl",
			"rsp", "esp", "sp", "spl",
			"r8", "r8d", "r8w", "r8b",
			"r9", "r9d", "r9w", "r9b",
			"r10", "r10d", "r10w", "r10b",
			"r11", "r11d", "r11w", "r11b",
			"r12", "r12d", "r12w", "r12b",
			"r13", "r13d", "r13w", "r13b",
			"r14", "r14d", "r14w", "r14b",
			"r15", "r15d", "r15w", "r15b",
			"xmm0", "ymm0",
			"xmm1", "ymm1",
			"xmm2", "ymm2",
			"xmm3", "ymm3",
			"xmm4", "ymm4",
			"xmm5", "ymm5",
			"xmm6", "ymm6",
			"xmm7", "ymm7",
			"xmm8", "ymm8",
			"xmm9", "ymm9",
			"xmm10", "ymm10",
			"xmm11", "ymm11",
			"xmm12", "ymm12",
			"xmm13", "ymm13",
			"xmm14", "ymm14",
			"xmm15", "ymm15"
		};

		public static bool IsRegister(string text)
		{
			return Registers.Contains(text);
		}

		private static HashSet<string> Keywords = new HashSet<string>
		{
			"ret", "retn",
			"call",
			"jl", "ja", "jb", "jbe", "je", "jz", "js", "jne", "jnz", "jns", "jae", "jmp",
			"push", "pop"
		};

		public static bool IsKeyword(string text)
		{
			return Keywords.Contains(text);
		}
	}
}
