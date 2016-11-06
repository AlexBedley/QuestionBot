﻿using System;
using System.Collections.Generic;

namespace QuestionBot.Model {
    public class ConsoleWrapper : IConsole {
        public string ReadLine() {
            return Console.ReadLine();
        }

        public void WriteLine(string message) {
            Console.WriteLine(message);
        }
    }
}