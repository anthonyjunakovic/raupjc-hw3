using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoRepository;

namespace TodoApp.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        [Required, MinLength(16), MaxLength(512)]
        public string Text { get; set; }
        public DateTime? Time { get; set; }
        public bool Completed { get; set; }
        public string Labels { get; set; }

        public TodoViewModel(Guid id, string text, DateTime? time, bool completed)
        {
            Id = id;
            Text = text;
            Time = time;
            Completed = completed;
            Labels = "";
        }

        public TodoViewModel(string text, DateTime? time, bool completed) : this(Guid.NewGuid(), text, time, completed)
        {
            
        }

        public TodoViewModel(string text, DateTime? time) : this(text, time, false)
        {

        }

        public TodoViewModel()
        {
            Labels = "";
        }

        /// <summary>
        /// Uses any input string and generates a color (in hex). Same strings always produce same colors.
        /// The logic behind this function is "dumb", and there is room for improvement, but I'm lazy. :)
        /// </summary>
        /// <param name="label"></param>
        /// <returns>Color (hex) that will be assigned to this string</returns>
        public static string GetLabelColor(string label)
        {
            label += "123"; // Assuring the minimum of 3 chars length
            int r = label[0], g = label[1], b = label[2];
            if ((r < 128) && (g < 128) && (b < 128))
            {
                // Some... random stuff, I guess
                if (r > g)
                {
                    b += 128;
                } else if (r > b)
                {
                    g += 128;
                } else if (g > b)
                {
                    r += 128;
                } else
                {
                    r += 128;
                    g += 128;
                }
            }
            return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }

        public static string GetLabelsRaw(IEnumerable<TodoItemLabel> labels)
        {
            string output = "";
            bool first = true;
            foreach (TodoItemLabel label in labels)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    output += ",";
                }
                output += label;
            }
            return output;
        }
    }
}
