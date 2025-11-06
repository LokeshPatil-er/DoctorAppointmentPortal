CREATE  OR ALTER  PROC dap_doctorsListAllOrBySpecialization 
 /*  
 -----------------------------------------------------------------------------------------------------------------------  
 Date			Created By		Purpose of creation  
 10 OCT 2025	Lokesh Patil	To get doctor list all or based on specialization  
 -----------------------------------------------------------------------------------------------------------------------  
 */  
  @SpecializationId INT = NULL    
AS    
BEGIN    
    SELECT    
        D.DoctorId,  
        D.FirstName + ' ' + D.LastName AS FullName,    
  D.ConsultancyFee,  
        ISNULL(STRING_AGG(Q.Degree, ', '), 'N/A') AS Qualifications    
    FROM DAP_Doctors D    
    INNER JOIN DAP_DoctorSpecializations DS    
        ON D.DoctorId = DS.DoctorId    
    LEFT JOIN DAP_DoctorQualifications DQ    
        ON D.DoctorId = DQ.DoctorId    
    LEFT JOIN DAP_Qualifications Q    
        ON DQ.QualificationId = Q.QualificationId    
    INNER JOIN DAP_Users U  
        ON D.UserId = U.UserId  
    INNER JOIN DAP_Roles R  
        ON U.RoleId = R.RoleId  
    WHERE    
        D.IsActive = 1              
        AND DS.IsActive = 1         
        AND R.IsActive = 1           
        AND (DQ.IsActive = 1 )  
        AND ( @SpecializationId IS NULL OR DS.SpecializationId = @SpecializationId )  
    GROUP BY    
        D.DoctorId,    
        D.FirstName,    
        D.LastName,  
		D.ConsultancyFee,  
        D.ExperienceStartDate  
    ORDER BY  
        D.ExperienceStartDate ASC;   
END;

GO