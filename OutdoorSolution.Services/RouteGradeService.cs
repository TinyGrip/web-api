using OutdoorSolution.Common;
using OutdoorSolution.Dto;
using OutdoorSolution.Services.Exceptions;
using OutdoorSolution.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services
{
    /// <summary>
    /// Performs convertion between supported climbing systems and DB grades storage types
    /// </summary>
    public class RouteGradeService : IRouteGradeService
    {
        // free climbing
        private readonly string[] YDS =    new string[] { "5.1", "5.2", "5.3", "5.4", "5.5", "5.6", "5.7", "5.8", "5.9", "5.10a", "5.10b", "5.10c", "5.10d", "5.11a", "5.11b",  "5.11c", "5.11d", "5.12a", "5.12b", "5.12c", "5.12d", "5.13a", "5.13b", "5.13c", "5.13d", "5.14a", "5.14b", "5.14c", "5.14d", "5.15a", "5.15b", "5.15c", "5.15d" };
        private readonly string[] FRENCH = new string[] { "2",   "2+",  "3",   "4a",  "4b",  "4c",  "5a",  "5b",  "5c",  "6a",    "6a+",   "6b",    "6b+",   "6b+",   "6c/6c+", "6c+",   "7a",    "7a+",   "7b",    "7b+",   "7c",    "7c+",   "8a",    "8a+",   "8b",    "8b+",   "8c",    "8c+",   "9a",    "9a+",   "9b",    "9b+",   "9c" };

        // bouldering
        private readonly string[] FRENCH_BOULDER = new string[] { "3",  "4-",  "4",  "4+",  "5",  "5+", "6A", "6A+", "6B", "6B+", "6C", "6C+", "7A", "7A+", "7B", "7B+", "7C", "7C+", "8A",  "8A+", "8B",  "8B+", "8C",  "8C+", "9A" };
        private readonly string[] HUECO          = new string[] { "VB", "V0-", "V0", "V0+", "V1", "V2", "V3", "V3",  "V4", "V4",  "V5", "V5",  "V6", "V7",  "V8", "V8",  "V9", "V10", "V11", "V12", "V13", "V14", "V15", "V16", "V17" };

        /// <summary>
        /// Converts string grade to its decimal equivalent, which is used in DB
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="routeType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public double GradeToComplexity(string grade, BoulderingGradesSystems system)
        {
            string[] gradesSystem = SelectBoulderingGrades(system);

            var complexity = Array.IndexOf(gradesSystem, grade);
            if (complexity < 0)
                throw new ArgumentException("Wrong input parameters, while converting grade to complexity");

            return complexity;
        }

        /// <summary>
        /// Converts string grade to its decimal equivalent, which is used in DB
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="routeType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public double GradeToComplexity(string grade, FreeClimbingGradesSystems system)
        {
            string[] gradesSystem = SelectFreeClimbingGrades(system);

            var complexity = Array.IndexOf(gradesSystem, grade);
            if (complexity < 0)
                throw new ArgumentException("Wrong input parameters, while converting grade to complexity");

            return complexity;
        }

        /// <summary>
        /// Converts decimal grade to its stadart string representation
        /// </summary>
        /// <param name="complexity"></param>
        /// <param name="routeType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public string ComplexityToGrade(double complexity, FreeClimbingGradesSystems system)
        {
            string[] gradesSystem = SelectFreeClimbingGrades(system);

            int gradeIndex = (int) Math.Round(complexity);
            if (gradeIndex < 0 || gradeIndex >= gradesSystem.Length)
            {
                throw new ArgumentException("Complexity value is not valid for provided system");
            }

            return gradesSystem[gradeIndex];
        }

        /// <summary>
        /// Converts decimal grade to its stadart string representation
        /// </summary>
        /// <param name="complexity"></param>
        /// <param name="routeType"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public string ComplexityToGrade(double complexity, BoulderingGradesSystems system)
        {
            string[] gradesSystem = SelectBoulderingGrades(system);

            int gradeIndex = (int)Math.Round(complexity);
            if (gradeIndex < 0 || gradeIndex >= gradesSystem.Length)
            {
                throw new ArgumentException("Complexity value is not valid for provided system");
            }

            return gradesSystem[gradeIndex];
        }


        private string[] SelectBoulderingGrades(BoulderingGradesSystems system)
        {
            if (system == BoulderingGradesSystems.French)
                return FRENCH_BOULDER;
            else if (system == BoulderingGradesSystems.Hueco)
                return HUECO;

            return null;
        }

        private string[] SelectFreeClimbingGrades(FreeClimbingGradesSystems system)
        {
            if (system == FreeClimbingGradesSystems.French)
                return FRENCH;
            else if (system == FreeClimbingGradesSystems.YDS)
                return YDS;

            return null;
        }

        /// <summary>
        /// Returns all available grades for boudlering system
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public string[] GetBoulderingGrades(BoulderingGradesSystems system)
        {
            string[] gradesSystem = SelectBoulderingGrades(system);
            return gradesSystem.Select(x => x).Distinct().ToArray();
        }

        /// <summary>
        /// Returns all available grades for free climbing system
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public string[] GetFreeClimbingGrades(FreeClimbingGradesSystems system)
        {
            string[] gradesSystem = SelectFreeClimbingGrades(system);
            return gradesSystem.Select(x => x).Distinct().ToArray();
        }
    }
}
