﻿using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public MathInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            leftNum = GetArgumentAt(0)?.EvaluateArgument().GetValue();
            rightNum = GetArgumentAt(1)?.EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return GetMathSymbol();
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(INumberDataType)
                },
                 new List<Type> {
                    typeof(INumberDataType)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left number", "Right Number" };
        }
    }
}
