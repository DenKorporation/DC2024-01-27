package com.github.hummel.dc.lab1.service

import com.github.hummel.dc.lab1.dto.request.IssueRequestTo
import com.github.hummel.dc.lab1.dto.request.IssueRequestToId
import com.github.hummel.dc.lab1.dto.response.IssueResponseTo

interface IssueService {
	suspend fun getAll(): List<IssueResponseTo>

	suspend fun create(issueRequestTo: IssueRequestTo?): IssueResponseTo?

	suspend fun deleteById(id: Long): Boolean

	suspend fun getById(id: Long): IssueResponseTo?

	suspend fun update(issueRequestToId: IssueRequestToId?): IssueResponseTo?
}