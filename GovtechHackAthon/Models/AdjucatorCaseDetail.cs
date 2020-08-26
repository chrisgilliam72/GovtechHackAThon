using GovtechDBLib.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{


    public class AdjucatorCaseDetail : CaseDetail
    {
        public bool IsDeferred { get; set; }
        public int AdjudicatorID { get; set; }
        public int CurrentRound { get; set; }
        public bool Submitted { get; set; }

        public CaseRatingInfo Rating { get; set; }

        public CaseActionsList ActionList { get; set; }


        public AdjucatorCaseDetail()
        {

            Rating = new CaseRatingInfo(CurrentRound);
            ActionList = new  CaseActionsList();
        }

        public static async  Task<AdjucatorCaseDetail> Load(int caseID, int UserID,int adjudicationRound, GovtechHackathonContext ctx)
        {
            var model = new AdjucatorCaseDetail();
            model.CurrentRound = adjudicationRound;
            ctx.Database.SetCommandTimeout(120);
            var caseInfo = await ctx.CaseInformation.Include("TeamMember").
                                                        Include("CaseAction").
                                                        Include("CaseAction.FkAdjudicator").
                                                        Include("CaseComment").
                                                        Include("CaseCategoryScore").
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
            model.CaseID = caseInfo.PkId;
            model.AdjudicatorID = UserID;
            model.CaseName = caseInfo.Name;
            model.Applicant = (ApplicantData)caseInfo.FkUser;
            model.Applicant.ViewOnlyMode = true;
            model.Submitted = caseInfo.IsAdjudicatorSubmitted;
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

            model.CompleteIdea = caseInfo.FkPurpose.CoreValues + "<br/>" + caseInfo.FkPurpose.Motivation + "<br/>";
            model.CompleteIdea += caseInfo.FkBusinessIdea.WhatProblem + caseInfo.FkBusinessIdea.How + "<br/>" + caseInfo.FkBusinessIdea.Who + "<br/>" + caseInfo.FkBusinessIdea.Impact;
            model.IsDeferred = caseInfo.FkStatus.Name == "Deferred";

            model.Rating = new CaseRatingInfo(adjudicationRound);
            model.Rating.CaseID = caseInfo.PkId;
            model.Rating.UserID = UserID;
            if (caseInfo.CaseComment != null)
            {
                var usrComment = caseInfo.CaseComment.FirstOrDefault(x => x.FkUserId ==UserID);
                model.Rating.Comment = usrComment != null ? usrComment.Comment : "";
            }
            if (caseInfo.CaseCategoryScore != null)
            {
                var ratingScores = caseInfo.CaseCategoryScore.Where(x => x.FkUserId == UserID && x.Round==model.CurrentRound).ToList();
                foreach (var ratingScore in ratingScores)
                {
                    var ratingCriteria = new CaseRatingCriteriaItem()
                    {
                        CategoryID = ratingScore.FkCategoryId,
                        ActualScore = ratingScore.Score,
                        Submitted = ratingScore.Submitted

                    };

                    model.Rating.RatingCriteria.Add(ratingCriteria);
                }
            }

            model.ActionList.CaseID = caseInfo.PkId;
            model.ActionList.AdjudicatorID = UserID;
            foreach (var caseAction in caseInfo.CaseAction)
            {
                var action = (CaseActionData)caseAction;
                model.ActionList.Actions.Add(action);
            }
            return model;
        }
    }
}
