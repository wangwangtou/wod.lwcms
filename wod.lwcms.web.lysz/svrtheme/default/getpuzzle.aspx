<%@ Page Language="C#" AutoEventWireup="true"%>
<% System.Data.Common.DbDataReader puzzle = PD.op.getObject<System.Data.Common.DbDataReader>("dr"); %>
<script type="text/javascript">
    var puzzles = {};
</script>
<h1>拼图列表</h1>
<ul>
<%
    if (puzzle != null)
    {
        using(puzzle)
	    {
            while (puzzle.Read())
            {
                %>
                <li>
                    <a href="javascript:;" puzzlename="<%=EcUrl(puzzle.GetString(puzzle.GetOrdinal("puzzle_name")))%>"><%=EcHtml(puzzle.GetString(puzzle.GetOrdinal("puzzle_name")))%></a>,上传人:<%=EcHtml(puzzle.GetString(puzzle.GetOrdinal("username")))%>
                    <script type="text/javascript">
                        puzzles["<%=EcUrl(puzzle.GetString(puzzle.GetOrdinal("puzzle_name")))%>"] = <%= puzzle.GetString(puzzle.GetOrdinal("puzzle_data"))%> ;
                    </script>
                </li>
                <%
            }
	    }
    }
 %>
 </ul>