using System.Collections.Generic;
using System.Linq;

namespace CommodoreBasicReformatter
{
    public class StmtsSplitter
    {
        internal void SplitLines(GrammarProgram program)
        {
            List<GrammarLine> astLines = program.Lines;
            for (int l = 0; l < astLines.Count; l++)
            {
                var line = astLines[l];

                // En az iki komut varsa split işlemi yapılabilir
                if (line.Stmts.Count > 1)
                {
                    int newLineNumber = line.LineNumber + 1;
                    int insertPos = l + 1;

                    while (line.Stmts.Count > 1)
                    {
                        // Güvenli kontrol: 2. komut var mı ve remark mı?
                        if (line.Stmts.Count > 1 &&
                            line.Stmts[1].Content.Count > 0 &&
                            line.Stmts[1].Content[0].IsRemark())
                        {
                            break;
                        }

                        // 1. komutun ilk token’ı IF mi?
                        if (line.Stmts[0].Content.Count > 0)
                        {
                            var token = line.Stmts[0].Content[0];
                            bool isIfThen = token.Type == TokenKind.Keyword && token.Value == "if";
                            if (isIfThen)
                                break;
                        }

                        // Diğer tüm komutlardan REM olmayanları al
                        var restOfLineExcludingRemark = line.Stmts
                            .Skip(1)
                            .Where(stmt => stmt.Content.Count > 0 && !stmt.Content[0].IsRemark())
                            .ToList();

                        // Yeni satırı AST’ye ekle
                        if (restOfLineExcludingRemark.Count > 0)
                        {
                            astLines.Insert(insertPos++, new GrammarLine(newLineNumber++, restOfLineExcludingRemark));
                        }

                        // İlk komut + varsa REM komutlarını tut
                        var firstStmtWithRemark = new[] { line.Stmts[0] }
                            .Concat(line.Stmts.Where(stmt => stmt.Content.Count > 0 && stmt.Content[0].IsRemark()))
                            .ToList();

                        line.Stmts = firstStmtWithRemark;
                    }
                }
            }
        }



    }
}