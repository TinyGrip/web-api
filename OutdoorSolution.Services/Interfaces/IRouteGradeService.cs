using OutdoorSolution.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutdoorSolution.Services.Interfaces
{
    public interface IRouteGradeService: IService
    {
        double GradeToComplexity(string grade, BoulderingGradesSystems system);

        double GradeToComplexity(string grade, FreeClimbingGradesSystems system);

        string ComplexityToGrade(double complexity, BoulderingGradesSystems system);

        string ComplexityToGrade(double complexity, FreeClimbingGradesSystems system);

        string[] GetBoulderingGrades(BoulderingGradesSystems system);

        string[] GetFreeClimbingGrades(FreeClimbingGradesSystems system);
    }
}
