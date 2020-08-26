using GovtechDBLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class InvestorCaseDetail : CaseDetail
    {
        public int InvestorID { get; set; }
        public bool IsInterested { get; set; }

        public static async Task<InvestorCaseDetail> Load(int caseID, int userID, GovtechHackathonContext ctx)
        {
            var model = new InvestorCaseDetail();
            ctx.Database.SetCommandTimeout(120);
            var caseInfo = await ctx.CaseInformation.Include("TeamMember").
                                                        Include("CaseComment").
                                                        Include("CaseCategoryScore").
                                                        Include("CaseInvestorInterested").
                                                        Include("TeamMember.FkRace").
                                                        Include("TeamMember.FkGender").
                                                        Include("TeamMember.FkCountry").
                                                        Include("TeamMember.FkProvince").
                                                        Include("FkSituationStatement").
                                                        Include("FkSituationOption").
                                                        Include("FkCompany").
                                                        Include("FkCompany.FkProvince").
                                                        Include("FkStudentInfo").
                                                        Include("CaseOutcomes").
                                                        Include("FkPurpose").
                                                        Include("FkStatus").
                                                        Include("FkBusinessIdea").
                                                        Include("CaseOutcomes.FkOutcome").
                                                        Include("FkUser").FirstOrDefaultAsync(x => x.PkId == caseID);
            var caseAssignment = await ctx.CaseAssignments.FirstOrDefaultAsync(x => x.FkCaseId == caseID);
            model.InvestorID = userID;
            model.CaseID = caseInfo.PkId;
            model.CaseName = caseInfo.Name;
            model.Applicant = (ApplicantData)caseInfo.FkUser;
            model.Applicant.ViewOnlyMode = true;
            model.TeamMembers = caseInfo.TeamMember.Select(x => (TeamMemberData)x).ToList();
            if (caseInfo.FkSituationOption.DisplayString != "I do not work for any of the below")
                model.SituationOption = caseInfo.FkSituationOption.DisplayString;
            model.SituationStatement = caseInfo.FkSituationStatement.DisplayString;
            if (caseInfo.FkCompany != null)
            {
                model.HasCompany = true;
                model.Company = new CompanyDetails()
                {
                    Name = caseInfo.FkCompany.Name,
                    Province = caseInfo.FkCompany.FkProvince.Name,
                    City = caseInfo.FkCompany.City
                };
            }
            if (caseInfo.FkStudentInfo != null)
            {
                model.IsStudent = true;
                model.StudentInfo = new StudentDetails()
                {
                    Course = caseInfo.FkStudentInfo.Course,
                    Instituion = caseInfo.FkStudentInfo.Instituion
                };
            }

            foreach (var outcome in caseInfo.CaseOutcomes)
                model.OutComes.Add(outcome.FkOutcome.Description);

            if (caseInfo.FkPurpose!=null && caseInfo.FkBusinessIdea!=null)
            {
                model.CompleteIdea = caseInfo.FkPurpose.CoreValues + "<br/>" + caseInfo.FkPurpose.Motivation + "<br/>";
                model.CompleteIdea += caseInfo.FkBusinessIdea.WhatProblem + caseInfo.FkBusinessIdea.How + "<br/>" + caseInfo.FkBusinessIdea.Who + "<br/>" + caseInfo.FkBusinessIdea.Impact;
            }

            if (caseInfo.CaseInvestorInterested == null || caseInfo.CaseInvestorInterested.Count==0)
                model.IsInterested = false;
            else
            {
                var investorInterested = caseInfo.CaseInvestorInterested.FirstOrDefault(x => x.FkInvestorId == userID);
                model.IsInterested = investorInterested!=null;
            }
               
            return model;
        }
    }
}
