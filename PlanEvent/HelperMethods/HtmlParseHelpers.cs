using PlanEvent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanEvent.HelperMethods
{
    public static class HtmlParseHelpers
    {
        public static bool TryParseStringActivityTimeInviteeTableIds(string str, out int InviteeIdnum, out int timeIdnum, out AVAILABILITY availability)
        {
            InviteeIdnum = 0;
            timeIdnum = 0;
            var sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '+':
                        if (int.TryParse(sb.ToString(), out InviteeIdnum))
                        {
                            sb.Clear();
                        }
                        break;
                    case 'x':
                        if (int.TryParse(sb.ToString(), out timeIdnum))
                        {
                            sb.Clear();
                        }
                        break;
                    default:
                        sb.Append(str[i]);
                        break;
                }
            }
            if (int.TryParse(sb.ToString(), out int num))
            {
                availability = (AVAILABILITY)num;
            }
            else
            {
                availability = AVAILABILITY.X;
            }

            return !(InviteeIdnum == 0 || timeIdnum == 0);
        }
    }
}
